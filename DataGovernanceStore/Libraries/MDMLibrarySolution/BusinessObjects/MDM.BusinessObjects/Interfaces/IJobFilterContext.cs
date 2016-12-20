using System;
using System.Runtime.Serialization;
using MDM.Core;

namespace MDM.BusinessObjects.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IJobFilterContext
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        Int32 JobId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        JobType JobType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        JobSubType JobSubType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        JobStatus JobStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        Boolean SkipJobDataLoading { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        DateTime? DateTimeFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        DateTime? DateTimeTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        Int32 Key { get; }

        /// <summary>
        /// 
        /// </summary>
        Boolean GetAll
        {
            get;
        }

        /// <summary>
        /// Indicates needs to get only specific user jobs or not
        /// </summary>
        Boolean GetOnlyUserJobs { get; set; }
    }
}