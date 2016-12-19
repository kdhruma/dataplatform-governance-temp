using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;


namespace MDM.ProfileManager.Business
{
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Imports;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Utility;

    public class CustomProfileBL : BusinessLogicBase
    {
        #region Fields

        #endregion

        #region CUD

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customProfile"></param>
        /// <param name="operationResult"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public OperationResult Create(CustomProfile customProfile, CallerContext callerContext)
        {
            ValidateProfile(customProfile, "Create");

            customProfile.Action = ObjectAction.Create;
            return Process(customProfile, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customProfile"></param>
        /// <param name="operationResult"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public OperationResult Update(CustomProfile customProfile, CallerContext callerContext)
        {
            ValidateProfile(customProfile, "Create");

            customProfile.Action = ObjectAction.Update;
            return Process(customProfile, callerContext);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customProfile"></param>
        /// <param name="operationResult"></param>
        /// <param name="programName"></param>
        /// <returns></returns>
        public OperationResult Delete(CustomProfile customProfile, CallerContext callerContext)
        {
            ValidateProfile(customProfile, "Create");

            customProfile.Action = ObjectAction.Delete;
            return Process(customProfile, callerContext);
        }

        /// <summary>
        /// Process custom profile based on action set for the object.
        /// </summary>
        /// <param name="customProfiles">The custom profile.</param>
        /// <param name="callerContext">The caller context.</param>
        /// <returns></returns>
        public OperationResultCollection Process(Collection<CustomProfile> customProfiles, CallerContext callerContext)
        {
            #region Validation

            if (customProfiles == null)
            {
                throw new MDMOperationException("222", "CustomProfile cannot be null.", "CustomProfileBL.Process", String.Empty, "Process");
            }

            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "CustomProfileBL.Process", String.Empty, "Process");
            }

            #endregion Validation

            OperationResultCollection operationResults = new OperationResultCollection();

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "MDM.ProfileManager.Business.CustomProfileBL.Process";
            }
            if (callerContext.Module == MDMCenterModules.Unknown)
            {
                callerContext.Module = MDMCenterModules.Import;
            }

            JobProfileBL jobProfileBL = new JobProfileBL();
            JobProfileCollection profiles = new JobProfileCollection();
            foreach (CustomProfile profile in customProfiles)
            {
                profile.ProfileDataXml = profile.ToXml();
                profiles.Add(( JobProfile )profile);
            }

            operationResults = jobProfileBL.Process(profiles, callerContext);

            return operationResults;
        }

        #endregion CUD

        #region Get

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public CustomProfile Get(Int32 profileId, MDMCenterApplication application)
        {
            CustomProfile customProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileId, application, MDMCenterModules.Import);

            if (jobProfile != null)
            {
                customProfile = new CustomProfile(jobProfile.ProfileDataXml); //TODO: Need a way to create job profile..
                //customProfile = (CustomProfile)jobProfile; //TODO: Need a way to create job profile..

                customProfile.Id = jobProfile.Id;
                customProfile.Name = jobProfile.Name;
            }
            
            return customProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public CustomProfile Get(String profileName, MDMCenterApplication application)
        {
            CustomProfile customProfile = null;
            JobProfile jobProfile = new JobProfileBL().Get(profileName, application, MDMCenterModules.Import);

            if (jobProfile != null)
            {
                customProfile = new CustomProfile(jobProfile.ProfileDataXml); //TODO: Need a way to create job profile..
                //customProfile = (CustomProfile)jobProfile; //TODO: Need a way to create job profile..

                customProfile.Id = jobProfile.Id;
                customProfile.Name = jobProfile.Name;
            }

            return customProfile;
        }
        
        #endregion Get

        #region Private Methods

        private void ValidateProfile(CustomProfile importProfile, String methodName)
        {
            if (importProfile == null)
            {
                throw new MDMOperationException("222", "CustomProfile cannot be null.", "CustomProfileBL." + methodName, String.Empty, methodName);
            }
        }

        private OperationResult Process(CustomProfile profile, CallerContext callerContext)
        {
            if (callerContext == null)
            {
                throw new MDMOperationException("111846", "CallerContext cannot be null.", "CustomProfileBL.Process", String.Empty, "Process");
            }

            OperationResultCollection operationResults = new OperationResultCollection();

            if (String.IsNullOrWhiteSpace(callerContext.ProgramName))
            {
                callerContext.ProgramName = "MDM.ProfileManager.Business.CustomProfileBL.Process";
            }
            if (callerContext.Module == MDMCenterModules.Unknown)
            {
                callerContext.Module = MDMCenterModules.Import;
            }

            JobProfileBL jobProfileBL = new JobProfileBL();
            profile.ProfileDataXml = profile.ToXml();

            operationResults = jobProfileBL.Process(new JobProfileCollection () { (JobProfile) profile}, callerContext);
            //Here expecting that jobProfileBL.Process() would have done the errorCode to errorMessage conversion so not doing here.
            return operationResults.FirstOrDefault();
        }

        #endregion
    }
}
