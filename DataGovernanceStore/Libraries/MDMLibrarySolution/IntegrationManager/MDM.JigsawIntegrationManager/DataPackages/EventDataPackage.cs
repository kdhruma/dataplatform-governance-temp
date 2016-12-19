using System;

namespace MDM.JigsawIntegrationManager.DataPackages
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.Core.Extensions;
    using MDM.BusinessObjects.Workflow;
    
    /// <summary>
    /// Represents class for EntityMessageDataPackage
    /// </summary>
    public class EventDataPackage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Entity
        /// </summary>
        public Entity SourceEntity{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EventSubType EventSubType { get; set; }

        /// <summary>
        /// "eventGroupId": "new GUID, representing current group / process triggering this even - ex. same guid for all events of particular export session",
        /// </summary>
        public Guid? EventGroupId { get; set;}

        /// <summary>
        /// 
        /// </summary>
        public String EventSourceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String ActingUser { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEventData EventData;

        #endregion
    }
}