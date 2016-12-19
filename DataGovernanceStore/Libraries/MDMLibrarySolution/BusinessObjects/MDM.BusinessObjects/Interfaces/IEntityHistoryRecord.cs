using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the entity history record.
    /// </summary>
    public interface IEntityHistoryRecord
    {
        #region Properties

        /// <summary>
        /// Property denoting the type of change happened
        /// </summary>
        EntityChangeType ChangeType { get; set; }

        ///     <summary>
        /// Property denoting details about what has been modified in an entity
        /// </summary>
        String Details { get; set; }

        /// <summary>
        /// Property denoting modified date and time - when changes have been done
        /// </summary>
        DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Property denoting modified user - who has done changes 
        /// </summary>
        String ModifiedUser { get; set; }

        /// <summary>
        /// Property denoting modified program - which program has done changes 
        /// </summary>
        String ModifiedProgram { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Represents IEntityHistoryRecord  in Xml format
        /// </summary>
        /// <returns>
        /// IEntityHistoryRecord  in Xml format
        /// </returns>
        String ToXml();

        #endregion
    }
}
