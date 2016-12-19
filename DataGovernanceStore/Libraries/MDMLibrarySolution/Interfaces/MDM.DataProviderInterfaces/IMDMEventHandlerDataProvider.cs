using System;
using System.Collections.ObjectModel;

namespace MDM.DataProviderInterfaces
{
    using MDM.Interfaces;

    /// <summary>
    /// Represents an interface, which provides methods to get MDMEventHandler data. 
    /// </summary>
    public interface IMDMEventHandlerDataProvider
    {
        /// <summary>
        /// Returns the MDMEventHandlerCollection based on the input identifiers specified
        /// </summary>
        /// <param name="eventHandlerIdList">The MDMEventHandler identifiers for which data has to be retrieved</param>
        /// <param name="callerContext">Context indicating the caller of the method</param>
        /// <returns>A collection of MDMEventHandler objects</returns>
        IMDMEventHandlerCollection GetMDMEventHandlers(Collection<Int32> eventHandlerIdList, ICallerContext callerContext);
    }
}
