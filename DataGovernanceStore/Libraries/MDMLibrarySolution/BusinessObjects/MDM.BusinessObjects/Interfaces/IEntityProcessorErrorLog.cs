using System;

namespace MDM.BusinessObjects.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the entity processor error log object.
    /// </summary>
    public interface IEntityProcessorErrorLog : IMDMObject
    {
        #region Properties

        /// <summary>
        ///  Property denoting the unique Id of the error log table
        /// </summary>
        new Int64 Id
        {
            get;
            set;
        }

        /// <summary>
        ///  Property denoting impacted entity Id
        /// </summary>
        Int64 ImpactedEntityId
        {
            get;
            set;
        }

        /// <summary>
        /// Property impacted indicating entity short name
        /// </summary>
        String ImpactedEntityName
        {
            get;
            set;
        }

        /// <summary>
        /// Property impacted indicating entity Long name
        /// </summary>
        String ImpactedEntityLongName
        {
            get;
            set;
        }

        /// <summary>
        ///  Property denoting priority of the imapcted entity
        /// </summary>
        Int32 Priority
        {
            get;
            set;
        }

        /// <summary>
        ///  Property denoting error message
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates the Type of denorm is used to indicate which denorm got error? 
        /// Enttiy Attribute denorm, Metadata denorm or relationship denorm
        /// </summary>
        String ProcessorName
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates Modified Date time
        /// </summary>
        DateTime? ModifiedDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Property indicates Modified User
        /// </summary>
        String ModifiedUser
        {
            get;
            set;
        }

        /// <summary>
        /// Property Indicated Modified Program
        /// </summary>
        String ModifiedProgram
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates the catalog Id of the Impacted entity
        /// </summary>
        Int32 ContainerId
        {
            get;
            set;
        }

        /// <summary>
        /// indicates the PK of Impacted Entity Log which is referred to impacted entity
        /// </summary>
        Int64 EntityActivityLogId
        {
            get;
            set;
        }

        /// <summary>
        /// Action performed by the processor
        /// </summary>
        EntityActivityList PerformedAction
        {
            get;
            set;
        }

        #endregion
    }
}
