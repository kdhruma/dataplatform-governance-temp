using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DQM
{
    using MDM.Interfaces;
    using MDM.Core;

    /// <summary>
    /// Specifies Job Report settings
    /// </summary>
    [DataContract]
    public class JobReportSettings : IJobReportSettings
    {
        #region Fields

        /// <summary>
        /// Field denotes collection of allowed job types
        /// </summary>
        private ICollection<DQMJobType> _jobTypes = new Collection<DQMJobType>();

        /// <summary>
        /// Field denotes CountFrom parameter for paged result 
        /// </summary>
        private Int64? _countFrom = null;

        /// <summary>
        /// Field denotes CountTo parameter for paged result
        /// </summary>
        private Int64? _countTo = null;

        /// <summary>
        /// Allows to request total items count including all pages
        /// </summary>
        private Boolean _requestTotalItemsCount = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public JobReportSettings()
        {
        } 

        #endregion

        #region Properties

        /// <summary>
        /// Denotes collection of allowed job types
        /// </summary>
        [DataMember]
        public ICollection<DQMJobType> JobTypes
        {
            get { return _jobTypes; }
            set { _jobTypes = value; }
        }

        /// <summary>
        /// Denotes CountFrom parameter for paged result 
        /// </summary>
        [DataMember]
        public Int64? CountFrom
        {
            get { return _countFrom; }
            set { _countFrom = value; }
        }

        /// <summary>
        /// Denotes CountTo parameter for paged result
        /// </summary>
        [DataMember]
        public Int64? CountTo
        {
            get { return _countTo; }
            set { _countTo = value; }
        }

        /// <summary>
        /// Allows to request total items count including all pages
        /// </summary>
        [DataMember]
        public Boolean RequestTotalItemsCount
        {
            get { return _requestTotalItemsCount; }
            set { _requestTotalItemsCount = value; }
        }

        #endregion
    }
}