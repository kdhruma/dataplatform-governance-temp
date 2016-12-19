using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get user preferences.
    /// </summary>
    public interface IUserPreferences : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Field denoting login name of the User
        /// </summary>
        String LoginName { get; set; }

        /// <summary>
        /// Field denoting login Identity of the User
        /// </summary>
        Int32 LoginId { get; set; }

        /// <summary>
        /// Field denoting default role Identity of the User
        /// </summary>
        Int32 DefaultRoleId { get; set; }

        /// <summary>
        /// Field denoting default role name of the User
        /// </summary>
        String DefaultRoleName { get; set; }

        /// <summary>
        /// Field denoting Type of the user
        /// </summary>
        String UserType { get; set; }

        /// <summary>
        /// Field denoting Initials of the user
        /// </summary>
        String UserInitials { get; set; }

        /// <summary>
        /// Field denoting data locale Identity of the user
        /// </summary>
        LocaleEnum DataLocale { get; set; }

        /// <summary>
        /// Field denoting UI locale Identity of the user
        /// </summary>
        LocaleEnum UILocale { get; set; }

        /// <summary>
        /// Field denoting Culture name of the Data 
        /// </summary>
        String DataCultureName { get; set; }

        /// <summary>
        /// Field denoting Culture name of the User
        /// </summary>
        String UICultureName { get; set; }

        /// <summary>
        /// Field denoting default organization Identity of the User 
        /// </summary>
        Int32 DefaultOrgId { get; set; }

        /// <summary>
        /// Field denoting default organization short name of the User 
        /// </summary>
        String DefaultOrgName { get; set; }

        /// <summary>
        /// Field denoting default organization Long name of the User 
        /// </summary>
        String DefaultOrgLongName { get; set; }

        /// <summary>
        /// Field denoting default Container Identity of the User 
        /// </summary>
        Int32 DefaultContainerId { get; set; }

        /// <summary>
        /// Field denoting default Container short name of the User 
        /// </summary>
        String DefaultContainerName { get; set; }

        /// <summary>
        /// Field denoting default container long name of the User 
        /// </summary>
        String DefaultContainerLongName { get; set; }

        /// <summary>
        /// Field denoting default hierarchy Identity of the User 
        /// </summary>
        Int32 DefaultHierarchyId { get; set; }

        /// <summary>
        /// Field denoting default hierarchy short name of the User 
        /// </summary>
        String DefaultHierarchyName { get; set; }

        /// <summary>
        /// Field denoting default hierarchy long name of the User 
        /// </summary>
        String DefaultHierarchyLongName { get; set; }

        /// <summary>
        /// Field denoting maximum rows on the table of the User
        /// </summary>
        Int32 MaxTableRows { get; set; }

        /// <summary>
        /// Field denoting maximum pages on the table of the User
        /// </summary>
        Int32 MaxTablePages { get; set; }

        /// <summary>
        /// Field denoting default time zone Identity of the User
        /// </summary>
        Int32 DefaultTimeZoneId { get; set; }

        /// <summary>
        /// Field denoting default time zone's short name of the User
        /// </summary>
        String DefaultTimeZoneShortName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of User Preferences
        /// </summary>
        /// <returns>Xml representation of User Preferences</returns>
        String ToXml();

        /// <summary>
        /// Clone User Preferences object
        /// </summary>
        /// <returns>Cloned copy of User Preferences object.</returns>
        IUserPreferences Clone();

        /// <summary>
        /// Delta Merge of User Preferences
        /// </summary>
        /// <param name="deltaUserPreferences">UserPreferences that needs to be merged.</param>
        /// <param name="iCallerContext">Indicates the name of application and the module that are performing the action</param>
        /// <param name="returnClonedObject">true means return new merged cloned object;otherwise, same object.</param>
        /// <returns>Merged UserPreferences instance</returns>
        IUserPreferences MergeDelta(IUserPreferences deltaUserPreferences, ICallerContext iCallerContext, Boolean returnClonedObject = true);

        #endregion
    }
}
