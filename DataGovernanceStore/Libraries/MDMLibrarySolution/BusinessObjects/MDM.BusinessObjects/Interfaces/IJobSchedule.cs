using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get job schedule criteria.
    /// </summary>
    public interface IJobSchedule : IMDMObject
    {
        /// <summary>
        /// Property denoting ScheduleCriteria
        /// </summary>
        ScheduleCriteria ScheduleCriteria { get; set; }
        
        /// <summary>
        /// Property denoting Profiles collection
        /// </summary>
        BaseProfileCollection Profiles { get; set; }

        /// <summary>
        /// Property denoting Computer Name
        /// </summary>
        String ComputerName { get; set; }
        
        /// <summary>
        /// Property denoting Job Schedule is enabled or not
        /// </summary>
        Boolean Enabled { get; set; }

        /// <summary>
        /// Property denoting last Run Date
        /// </summary>
        DateTime? LastRunDate { get; set; }

        /// <summary>
        /// Property denoting Last Run Status
        /// </summary>
        String LastRunStatus { get; set; }

        /// <summary>
        /// Property denoting Next Run Date
        /// </summary>
        DateTime? NextRunDate { get; set; }

        /// <summary>
        /// Property denoting last modified dateTime
        /// </summary>
        DateTime? LastModofiedDateTime { get; set; }

        /// <summary>
        /// Property denoting Create User Name
        /// </summary>
        String CreateUserName { get; set; }

        /// <summary>
        /// Get Scheduled Criteria
        /// </summary>
        /// <returns>IScheduleCriteria</returns>
        IScheduleCriteria GetScheduleCriteria();

        /// <summary>
        /// Get Profiles
        /// </summary>
        /// <returns>Collection of BaseProfiles</returns>
        MDM.BusinessObjects.BaseProfileCollection GetProfiles();
        
        /// <summary>
        /// Add Profile
        /// </summary>
        /// <param name="profile">Profile to add</param>
        void AddProfile(IBaseProfile profile);

        /// <summary>
        /// XML presentation of an Object
        /// </summary>
        /// <returns>XML presentation of an Object</returns>
        String ToXml();
    }
}
