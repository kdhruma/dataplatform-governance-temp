using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get caller context.
    /// </summary>
    public interface ICallerContext  
    {
        #region Properties

        /// <summary>
        /// Indicates the MDM Event Source
        /// </summary>
        EventSource MDMSource
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the MDM Subscriber
        /// </summary>
        EventSubscriber MDMSubscriber
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the MDM Publisher
        /// </summary>
        MDMPublisher MDMPublisher
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the Application of MDMCenter
        /// </summary>
        MDMCenterApplication Application
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the  Module of MDMCenter
        /// </summary>
        MDMCenterModules Module
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the IEntityChangeContext
        /// </summary>
        String ProgramName { get; }

        /// <summary>
        /// Indicates the Job Id
        /// </summary>
        Int32 JobId
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the Profile Id
        /// </summary>
        Int32 ProfileId
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the Profile Name
        /// </summary>
        String ProfileName
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the Operation Id
        /// </summary>
        Guid OperationId
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the Dagnostic Activity Id of the caller who initiated this call. 
        /// </summary>
        Guid ActivityId
        {
            get;
            set;
        }
        
        /// <summary>
        /// Indicates the trace settings
        /// </summary>
        TraceSettings TraceSettings
        {
            get;
            set;
        }

        #endregion

        #region Methods

        #endregion
    }
}
