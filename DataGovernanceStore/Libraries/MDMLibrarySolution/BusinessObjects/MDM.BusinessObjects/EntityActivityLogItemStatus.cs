using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;   
    using MDM.Interfaces;

    /// <summary>
    /// Specifies entity activity log item status for data quality management system
    /// </summary>
    [DataContract]
    public class EntityActivityLogItemStatus : ObjectBase, IEntityActivityLogItemStatus
    {
        #region Fields

        /// <summary>
        /// Field indicates EntityActivityLog record Id
        /// </summary>
        private Int64 _entityActivityLog = -1;

        /// <summary>
        /// Field indicates the activity action
        /// </summary>
        private EntityActivityList _performedAction = EntityActivityList.UnKnown;

        /// <summary>
        /// Field indicates whether loading of impacted entities is in progress
        /// </summary>
        private Boolean? _isLoadingInProgress = null;

        /// <summary>
        /// Field indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        private Boolean? _isLoaded = null;

        /// <summary>
        /// Field indicates whether all the loaded impacted entites in the impacted Entity table has processed
        /// </summary>
        private Boolean? _isProcessed = null;

        /// <summary>
        /// Field indicates the time when the impacted entities loading in to impacted Entity table for processing started
        /// </summary>
        private DateTime? _loadStartTime = null;

        /// <summary>
        /// Field indicates the time when the impacted entities loading in to impacted Entity table for processing ended
        /// </summary>
        private DateTime? _loadEndTime = null;

        /// <summary>
        /// Field indicates the processing start time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        private DateTime? _processStartTime = null;

        /// <summary>
        /// Field indicates the processing end time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        private DateTime? _processEndTime = null;

        /// <summary>
        /// Field indicates the Created Date time of the entites in the Activity log table 
        /// </summary>
        private DateTime? _createdDateTime = null;

        /// <summary>
        /// Field indicates the number of processed entities related to EntityActivityLog item
        /// </summary>
        private Int64 _processedEntitiesCount = 0;

        /// <summary>
        /// Field indicates the total number of entities related to EntityActivityLog item
        /// </summary>
        private Int64 _totalEntitiesCount = 0;

        /// <summary>
        /// Field indicates the total number of entities related to EntityActivityLog item
        /// </summary>
        private String _userName = String.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Property indicates EntityActivityLog record Id
        /// </summary>
        [DataMember]
        public Int64 EntityActivityLogId
        {
            get { return _entityActivityLog; }
            set { _entityActivityLog = value; }
        }

        /// <summary>
        /// Indicates the activity action
        /// </summary>
        [DataMember]
        public EntityActivityList PerformedAction
        {
            get { return _performedAction; }
            set { _performedAction = value; }
        }

        /// <summary>
        /// Property indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        [DataMember]
        public Boolean? IsLoadingInProgress
        {
            get { return this._isLoadingInProgress; }
            set { this._isLoadingInProgress = value; }
        }

        /// <summary>
        /// Property indicates whether all the impacted entites has been loaded in to impacted Entity table for processing
        /// </summary>
        [DataMember]
        public Boolean? IsLoaded
        {
            get { return this._isLoaded; }
            set { this._isLoaded = value; }
        }

        /// <summary>
        ///  Property indicates whether all the loaded impacted entites in the impacted Entity table has processed
        /// </summary>
        [DataMember]
        public Boolean? IsProcessed
        {
            get { return this._isProcessed; }
            set { this._isProcessed = value; }
        }

        /// <summary>
        /// Property indicates the time when the impacted entities loading in to impacted Entity table for processing started
        /// </summary>
        [DataMember]
        public DateTime? LoadStartTime
        {
            get { return this._loadStartTime; }
            set { this._loadStartTime = value; }
        }

        /// <summary>
        /// Property indicates the time when the impacted entities loading in to impacted Entity table for processing ended
        /// </summary>
        [DataMember]
        public DateTime? LoadEndTime
        {
            get { return this._loadEndTime; }
            set { this._loadEndTime = value; }
        }

        /// <summary>
        /// Property indicates the processing start time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        [DataMember]
        public DateTime? ProcessStartTime
        {
            get { return this._processStartTime; }
            set { this._processStartTime = value; }
        }

        /// <summary>
        ///  Property indicates the processing end time of the loaded impacted entites in the impacted Entity table 
        /// </summary>
        [DataMember]
        public DateTime? ProcessEndTime
        {
            get { return this._processEndTime; }
            set { this._processEndTime = value; }
        }

        /// <summary>
        /// Property indicates the Created Date time of Activity log table record
        /// </summary>
        [DataMember]
        public DateTime? CreatedDateTime
        {
            get { return this._createdDateTime; }
            set { this._createdDateTime = value; }
        }

        /// <summary>
        /// Property indicates the number of processed entities related to EntityActivityLog item
        /// </summary>
        [DataMember]
        public Int64 ProcessedEntitiesCount
        {
            get { return this._processedEntitiesCount; }
            set { this._processedEntitiesCount = value; }
        }

        /// <summary>
        /// Property indicates the total number of entities related to EntityActivityLog item
        /// </summary>
        [DataMember]
        public Int64 TotalEntitiesCount
        {
            get { return this._totalEntitiesCount; }
            set { this._totalEntitiesCount = value; }
        }

        /// <summary>
        /// Property indicates the EntityActivityLog item State
        /// </summary>
        [IgnoreDataMember]
        public EntityActivityLogItemState State
        {
            get
            {
                if (_isLoadingInProgress.HasValue || _isLoaded.HasValue || _isProcessed.HasValue)
                {
                    if (_isProcessed == true)
                    {
                        return EntityActivityLogItemState.Processed;
                    }
                    if (_isLoaded == true)
                    {
                        return EntityActivityLogItemState.Processing;
                    }
                    if (_isLoadingInProgress == true)
                    {
                        return EntityActivityLogItemState.Loading;
                    }
                    return EntityActivityLogItemState.Initial;
                }
                return EntityActivityLogItemState.UnKnown;
            }
        }

        /// <summary>
        /// Property indicates the launcher of job 
        /// </summary>
        [DataMember]
        public String UserName 
        {
            get { return _userName; }
            set { _userName = value; }
        }

        #endregion Properties

        #region Constructors        

        #endregion Constructors        
    }
}
