using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    /// <summary>
    /// Contains properties and methods to manipulate event info collection object
    /// </summary>
    public interface IMDMEventInfoCollection : ICollection<IMDMEventInfo>, ICloneable
    {
        #region Methods

        /// <summary>
        /// Get XML representation of event info collection object
        /// </summary>
        /// <returns>XML representation of event info collection object</returns>
        String ToXml();

        /// <summary>
        /// Gets a cloned instance of the current event info collection object
        /// </summary>
        /// <returns>Cloned instance of the current event info collection object</returns>
        new IMDMEventInfoCollection Clone();

        /// <summary>
        /// Gets a MDMEventInfo for the requested Id
        /// </summary>
        /// <param name="eventInfoId">Indicates the eventInfo Id</param>
        /// <returns>Returns a MDMEventInfo object.</returns>
        IMDMEventInfo GetEventInfoById(Int32 eventInfoId);

        #endregion  Methods
    }
}
