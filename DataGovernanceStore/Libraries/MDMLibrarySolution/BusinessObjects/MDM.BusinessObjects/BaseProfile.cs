using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.BusinessObjects.DQM;
    using MDM.BusinessObjects.DQMNormalization;
    using MDM.BusinessObjects.Exports;
    using MDM.Core;
    using MDM.Core.Exceptions;
    using MDM.Interfaces;

    /// <summary>
    /// Base class for all profiles
    /// </summary>
    [DataContract]
    [KnownType(typeof(ExportProfile))]
    [KnownType(typeof(ValidationProfile))]
    [KnownType(typeof(NormalizationProfile))]
    [KnownType(typeof(MergingProfile))]
    [KnownType(typeof(MatchingProfile))]
    public abstract class BaseProfile : MDMObject, IBaseProfile
    {
        #region Fields
        
        [DataMember]
        private ProfileType? _profileType = null;

        #endregion

        /// <summary>
        /// Creates specific profile based on Profile type
        /// </summary>
        /// <param name="profileType">Profile type</param>
        /// <returns>Returns concrete profile instance</returns>
        public static BaseProfile CreateProfile(ProfileType profileType)
        {
            //TODO: possibly use reflection and type name as a parameter
            BaseProfile profile;

            switch (profileType)
            {
                case ProfileType.Export:
                    profile = new ExportProfile();
                    break;
                case ProfileType.Validation:
                    profile = new ValidationProfile();
                    break;
                case ProfileType.Normalization:
                    profile = new NormalizationProfile();
                    break;
                case ProfileType.Merging:
                    profile = new MergingProfile();
                    break;
                case ProfileType.Matching:
                    profile = new MatchingProfile();
                    break;
                default:
                    throw new MDMOperationException(String.Empty, String.Format("Profile type {0} is not supported", profileType), "BaseProfile.CreateProfile", String.Empty, "CreateProfile"); //TODO: localize
            }

            return profile;
        }

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        protected BaseProfile()
        {
        }

        /// <summary>
        /// Constructor with Id, Name and LongName for an base profile as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Object</param>
        /// <param name="name">Indicates the Name of an Object</param>
        /// <param name="longName">Indicates the LongName of an Object</param>
        protected BaseProfile(Int32 id, String name, String longName)
            : base(id, name, longName)
        {
        }

        /// <summary>
        /// Constructor with Id, Name, LongName, auditRefId and programName of an Localized value for an base profile as input parameters
        /// </summary>
        /// <param name="id">Indicates the Identity of an Object</param>
        /// <param name="name">Indicates the Name of an Object</param>
        /// <param name="longName">Indicates the LongName of an Object</param>
        /// <param name="locale">Indicates the Locale of an Object</param>
        /// <param name="auditRefId">Indicates the AuditRefId of an Object</param>
        /// <param name="programName">Indicates programName of an Object</param>
        protected BaseProfile(Int32 id, String name, String longName, LocaleEnum locale, Int64 auditRefId, String programName)
            : base(id, name, longName, locale, auditRefId, programName)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting profile type
        /// </summary>
        public ProfileType ProfileType
        {
            get
            {
                if (_profileType == null)
                {
                    _profileType = GetProfileType();
                }

                return (ProfileType)_profileType;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the type of the profile
        /// </summary>
        /// <returns>Returns ProfileType of the profile</returns>
        public ProfileType GetProfileType()
        {
            //TODO: possibly use object.GetType.ToString() to obtain type name string
            ProfileType profileType = ProfileType.Unknown;

            if (this is ExportProfile)
                profileType = ProfileType.Export;
            else if (this is ValidationProfile)
                profileType = ProfileType.Validation;
            else if (this is DQMNormalization.NormalizationProfile)
                profileType = ProfileType.Normalization;
            else if (this is MergingProfile)
                profileType = ProfileType.Merging;
            else if (this is MatchingProfile)
                profileType = ProfileType.Matching;
            else
            {
                throw new MDMOperationException(String.Empty, String.Format("Profile type {0} is not supported", this.GetType().Name), "BaseProfile.GetProfileType", String.Empty, "GetProfileType"); // TODO: localize
            }

            return profileType;
        }

        #endregion

    }
}
