using System;
using System.CodeDom;
using System.Diagnostics;
using System.Linq;

namespace MDM.ProfileManager.Business
{
    using MDM.Core;
    using MDM.BusinessObjects;

    using MDM.Utility;
    using MDM.Core.Exceptions;
    using MDM.AdminManager.Business;
    using MDM.MessageManager.Business;
    using MDM.BusinessObjects.Diagnostics;
    using MDM.BusinessObjects.Exports;
    using System.Collections.ObjectModel;
    using MDM.ConfigurationManager.Business;
    using MDM.ProfileManager.Data;
    using MDM.BusinessObjects.Imports;
    using System.Xml;
    //using MDM.BusinessObjects.Instrumentation;

    /// <summary>
    /// Specifies the business logic class for Diagnostic Profile
    /// </summary>
    public class DiagnosticReportProfileBL : BusinessLogicBase
    {
        #region Fields

        /// <summary>
        /// Field denoting the security principal
        /// </summary>
        private SecurityPrincipal _securityPrincipal = null;


        /// <summary>
        /// Field denoting key value to decide whether to enable or disable the security permissions for diagnosticReport profile.
        /// </summary>
        private Boolean _isProfileManagerPermissionsEnabled = true;

        /// <summary>
        /// Indicates current trace settings
        /// </summary>
        private TraceSettings _traceSettings = new TraceSettings();

        #endregion

        #region Constructors

        public void DiagnosticProfileBL()
        {
            GetSecurityPrincipal();
            _isProfileManagerPermissionsEnabled = AppConfigurationHelper.GetAppConfig<bool>("MDMCenter.DiagnosticReportProfileManager.Permissions.Enabled", true);
            _traceSettings = MDMOperationContextHelper.GetCurrentTraceSettings();
        }

        #endregion Constructors

        #region CUD

        /// <summary>
        /// Creates the specified diagnosticReport profile.
        /// </summary>
        /// <param name="reportProfile">Indicates the diagnosticReport profile.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules.</param>
        /// <returns>Returns operation result collection when trying to create the given diagnosticReport profile collection</returns>
        public OperationResult Create(DiagnosticReportProfile reportProfile, CallerContext callerContext)
        {
            ValidateInputParameters(reportProfile, callerContext);
            reportProfile.Action = ObjectAction.Create;
            return Process(reportProfile, callerContext); 
        }




        /// <summary>
        /// Updates the specified diagnosticReport profile.
        /// </summary>
        /// <param name="diagnosticReportProfile">Indicates the diagnosticReport profile.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules</param>
        /// <returns></returns>
        public OperationResult Update(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext)
        {
            ValidateInputParameters(diagnosticReportProfile, callerContext);
            diagnosticReportProfile.Action = ObjectAction.Update;
            return Process(diagnosticReportProfile, callerContext);
        }

        /// <summary>
        /// Deletes the specified diagnosticReport profile.
        /// </summary>
        /// <param name="diagnosticReportProfile">Indicates the diagnosticReport profile.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules</param>
        /// <returns>Returns operation result collection when trying to delete the given diagnosticReport profile collection.</returns>
        public OperationResult Delete(DiagnosticReportProfile diagnosticReportProfile, CallerContext callerContext)
        {
            ValidateInputParameters(diagnosticReportProfile, callerContext);

            diagnosticReportProfile.Action = ObjectAction.Delete;
            return Process(diagnosticReportProfile, callerContext);
        }


        /// <summary>
        /// Returns operation result collection after processing the given diagnosticReport profile collection.
        /// </summary>
        /// <param name="reportProfileCollection"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        public OperationResultCollection Process(DiagnosticReportProfileCollection reportProfileCollection, CallerContext callerContext)
        {
            DiagnosticActivity activity = new DiagnosticActivity();
            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }
            #region Validation

            if (reportProfileCollection == null)
            {
                const String errorMessage = "reportProfileCollection cannot be null";
                activity.LogError("112456", errorMessage);
                throw new MDMOperationException("112456", errorMessage, "DiagnosticReportProfileBL.Process", String.Empty, "Process");
            }


            ValidateCallerContext(callerContext);

            #endregion Validation

            OperationResultCollection operationResults = new OperationResultCollection();

            foreach (DiagnosticReportProfile profile in reportProfileCollection)
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
        /// <returns>Returns the diagnosticReport profile based on profile Id</returns>
        public DiagnosticReportProfile Get(Int32 profileId, CallerContext callerContext)
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            #region Check input parameters
            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            if (profileId < 1)
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogError("ProfileId parameter is not valid.");
                }

                return null;
            }

            if (callerContext == null)
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogError("callerContext parameter can not be null or empty.");
                }
                
                return null;
            }

            #endregion

            DiagnosticReportProfile reportProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileId, callerContext.Application, MDMCenterModules.Instrumentation);

            if (jobProfile != null)
            {
                reportProfile = new DiagnosticReportProfile(jobProfile.ProfileDataXml);

                reportProfile.Id = jobProfile.Id;
                reportProfile.Name = jobProfile.Name;
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }

            return reportProfile;
        }

        /// <summary>
        /// Gets the specified profile by name.
        /// </summary>
        /// <param name="profileName">Indicates the name of the profile.</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules.</param>
        /// <returns>Returns the diagnosticReport profile by profile Name</returns>
        public DiagnosticReportProfile Get(String profileName, CallerContext callerContext)
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start();
            }

            #region Check input parameters

            if (String.IsNullOrEmpty(profileName))
            {
                activity.LogError("ProfileName cannot be null or empty");
                return null;
            }

            if (callerContext == null)
            {
                activity.LogError("CallerContext cannot be null or empty");
                return null;
            }

            #endregion

            DiagnosticReportProfile reportProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileName, callerContext.Application, MDMCenterModules.Instrumentation);

            if (jobProfile != null)
            {
                reportProfile = new DiagnosticReportProfile(jobProfile.ProfileDataXml)
                {
                    Id = jobProfile.Id,
                    Name = jobProfile.Name
                }; 
            }

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Stop();
            }

            return reportProfile;
        }

        /// <summary>
        /// Gets all diagnosticReport profiles.
        /// </summary>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules.</param>
        /// <returns></returns>
        public DiagnosticReportProfileCollection GetAll(CallerContext callerContext)
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.Start(new ExecutionContext(callerContext, new CallDataContext(), new SecurityContext(), string.Empty));
                activity.OperationId = Constants.ProfileTracingOperationId;
            }

            #region Check input parameters

            ValidateCallerContext(callerContext);

            #endregion

            DiagnosticReportProfileCollection diagnosticReportProfileCollection = new DiagnosticReportProfileCollection();

            DBCommandProperties command = DBCommandHelper.Get(MDMCenterApplication.MDMCenter, MDMCenterModules.Instrumentation, MDMCenterModuleAction.Read);

            JobProfileCollection jobProfileCollection = new JobProfileDA().GetAll(command, "DiagnosticReportExport");

            if (jobProfileCollection != null)
            {
                foreach (JobProfile jobProfile in jobProfileCollection)
                {
                    DiagnosticReportProfile diagnosticReportProfile = new DiagnosticReportProfile(jobProfile.ProfileDataXml);

                    diagnosticReportProfile.Id = jobProfile.Id;
                    diagnosticReportProfile.Name = jobProfile.Name;
                    diagnosticReportProfile.ProfileDomain = jobProfile.ProfileDomain;
                    diagnosticReportProfile.FileType = jobProfile.FileType;
                    diagnosticReportProfile.IsSystemProfile = jobProfile.IsSystemProfile;
                    diagnosticReportProfile.JobType = jobProfile.JobType;
                    diagnosticReportProfile.LastModified = jobProfile.LastModified;

                    diagnosticReportProfileCollection.Add(diagnosticReportProfile);
                }
            }

            if (_traceSettings.IsBasicTracingEnabled) activity.Stop();

            return diagnosticReportProfileCollection;
        }

        /// <summary>
        /// Get Profile type by id
        /// </summary>
        /// <param name="profileId">Indicates the identifier of profile</param>
        /// <param name="callerContext">Indicates the caller context specifying the application and modules.</param>
        /// <returns>Returns the type of profile based on profile Id.</returns>
        public String GetProfileType(Int32 profileId, CallerContext callerContext)
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.Start();
                }

            #region Check input parameters

            if (profileId < 1)
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogError("ProfileId parameter is not valid.");
                }
                return null;
            }

            if (callerContext == null)
            {

                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogError("callerContext parameter can not be null or empty.");
                }

                return null;
            }

            #endregion

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogVerbose("Start : MDM.ProfileManager.Business.DiagnosticReportProfileBL.GetProfileType");
            }
            

            #region Populate and Validate User Permission

            //If _isProfileManagerPermissionsEnabled key value is true, then it follows the security permissions, otherwise bypass the permissions.
            //Added this key to bypass the security permissions for VP for diagnosticReport profile.
            if (_isProfileManagerPermissionsEnabled && !ValidateUserPermission())
            {
                if (_traceSettings.IsBasicTracingEnabled)
                {
                    activity.LogError("101146", "You do not have sufficient permission to perform this operation");
                }

                throw new MDMOperationException("101146", "You do not have sufficient permission to perform this operation", "DiagnosticReportProfileManager.GetProfileType ", String.Empty, "Get");
            }

            #endregion Validate User Permission

            JobProfile jobProfile = new JobProfileBL().Get(profileId, callerContext.Application, MDMCenterModules.Instrumentation);

            if (_traceSettings.IsBasicTracingEnabled)
            {
                activity.LogVerbose("End : MDM.ProfileManager.Business.DiagnosticReportProfileBL.GetProfileType");
                activity.Stop();
            }

            return jobProfile == null ? String.Empty : jobProfile.FileType;
        }

        #endregion Get

        #region Private Methods

        private OperationResult Process(DiagnosticReportProfile reportProfile, CallerContext callerContext)
        {
            #region Validate User Permission

            //If _isProfileManagerPermissionsEnabled key value is true, then it follows the security permissions, otherwise bypass the permissions.
            //Added this key to bypass the security permissions for VP for diagnosticReport profile.
            if (_isProfileManagerPermissionsEnabled && !ValidateUserPermission())
            {
                OperationResult permissionOperationResult = new OperationResult();
                PopulateOperationResult(reportProfile, permissionOperationResult, callerContext, true);
                return permissionOperationResult;
            }

            #endregion Validate User Permission

            OperationResultCollection operationResults = new OperationResultCollection();

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "MDM.ProfileManager.Business.DiagnosticReportProfileBL.Process";
            }
            if (callerContext.Module == MDMCenterModules.Unknown)
            {
                callerContext.Module = MDMCenterModules.Instrumentation;
            }

            JobProfileBL jobProfileBL = new JobProfileBL();
            reportProfile.ProfileDataXml = reportProfile.ToXml();

            operationResults = jobProfileBL.Process(new JobProfileCollection() { (JobProfile) reportProfile }, callerContext);
            //Here expecting that jobProfileBL.Process() would have done the errorCode to errorMessage conversion so not doing here.
            return operationResults.FirstOrDefault();
        }

        private void ValidateInputParameters(DiagnosticReportProfile reportProfile, CallerContext callerContext)
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            if (reportProfile == null)
            {
                const String errorMessage = "reportProfile cannot be null";
                activity.LogError(errorMessage);
                throw new MDMOperationException("112455", errorMessage, "DiagnosticReportProfileBL.Process", String.Empty, "Process");
            }

            ValidateCallerContext(callerContext);
        }

        private void ValidateCallerContext(CallerContext callerContext)
        {
            if (callerContext == null)
            {
                DiagnosticActivity activity = new DiagnosticActivity();
                const String errorMessage = "CallerContext cannot be null";
                activity.LogError(errorMessage);
                throw new MDMOperationException("111846", errorMessage, "DiagnosticReportProfileBL.Process", String.Empty, "Process");
            }
        }

        private void GetSecurityPrincipal()
        {
            if (_securityPrincipal == null)
            {
                _securityPrincipal = SecurityPrincipalHelper.GetCurrentSecurityPrincipal();
            }
        }

        private static void PopulateOperationResult(DiagnosticReportProfile reportProfile, OperationResult diagnosticReportProfileProcessOperationResult, CallerContext callerContext, Boolean populatePermissionError = false)
        {
            DiagnosticActivity activity = new DiagnosticActivity();

            if (populatePermissionError)
            {

                LocaleMessage localeMessage = null;
                LocaleMessageBL localeMessageBL = new LocaleMessageBL();
                String profileReference = String.IsNullOrEmpty(reportProfile.ReferenceId)
                ? reportProfile.Name
                : reportProfile.ReferenceId;
                Object[] param = new Object[] { reportProfile.Action.ToString(), profileReference };

                localeMessage = localeMessageBL.Get(GlobalizationHelper.GetSystemUILocale(), "112291", param, false, callerContext);

                String errorMessage = localeMessage.Message;
                diagnosticReportProfileProcessOperationResult.AddOperationResult("112291", errorMessage, OperationResultType.Error);
                activity.LogError(errorMessage);
            }
            else
            {
                if (reportProfile.Action == ObjectAction.Create)
                {
                    if (diagnosticReportProfileProcessOperationResult.ReturnValues.Any())
                    {
                        diagnosticReportProfileProcessOperationResult.Id =
                            Convert.ToInt32(diagnosticReportProfileProcessOperationResult.ReturnValues[0]);
                    }
                }
                else
                {
                    diagnosticReportProfileProcessOperationResult.Id = reportProfile.Id;
                }
            }

            diagnosticReportProfileProcessOperationResult.ReferenceId = String.IsNullOrEmpty(reportProfile.ReferenceId)
                ? reportProfile.Name
                : reportProfile.ReferenceId;

        }

        private Boolean ValidateUserPermission()
        {
            Permission permission = null;
            DataSecurityBL dataSecurityManager = new DataSecurityBL();
            Int32 objectTypeId = (Int32)ObjectType.Catalog;

            PermissionContext permissionContext = new PermissionContext(0, 0, 0, 0, 0, 0, 0, 0, SecurityPrincipalHelper.GetCurrentSecurityPrincipal().CurrentUserId, 0);
            permission = dataSecurityManager.GetMDMObjectPermission(0, objectTypeId, ObjectType.Catalog.ToString(), permissionContext);

            return (permission != null && permission.PermissionSet.Contains(UserAction.Export));
        }

        /// <summary>
        /// check whether profile with such name already exist
        /// </summary>
        /// <param name="reportProfile"></param>
        /// <param name="callerContext"></param>
        /// <returns></returns>
        private bool IsExist(DiagnosticReportProfile reportProfile, CallerContext callerContext)
        {
            DiagnosticReportProfile targetProfile = Get(reportProfile.Name, callerContext);
            if (targetProfile == null)
            {
                return false;
            }
            else return true;

        }

        #endregion
    }
}

