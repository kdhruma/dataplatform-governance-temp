using System;
using System.Collections.ObjectModel;

namespace MDM.ConfigurationManager.Business
{
    using Interfaces;
    using BusinessObjects;
    using DataProviderInterfaces;

    /// <summary>
    /// Represents an implementation of the IMDMEventHandlerDataProvider using Business Logic call
    /// </summary>
    public class MDMEventHandlerDataProviderUsingDB : IMDMEventHandlerDataProvider
    {
        #region IMDMEventHandlerDataProvider Methods

        /// <summary>
        /// Returns the MDMEventHandlerCollection based on the input identifiers specified
        /// </summary>
        /// <param name="eventHandlerIdList">The MDMEventHandler identifiers for which data has to be retrieved</param>
        /// <param name="callerContext">Context indicating the caller of the method</param>
        /// <returns>A collection of MDMEventHandler objects</returns>
        public IMDMEventHandlerCollection GetMDMEventHandlers(Collection<Int32> eventHandlerIdList, ICallerContext callerContext)
        {
            MDMEventHandlerBL eventHandlerBL = new MDMEventHandlerBL();
            return eventHandlerBL.Get(eventHandlerIdList, callerContext as CallerContext);
        }

        #endregion IMDMEventHandlerDataProvider Methods
    }
}
