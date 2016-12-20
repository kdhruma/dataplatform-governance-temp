using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.Core;
    using MDM.BusinessObjects;
    using MDM.BusinessObjects.Diagnostics;

    /// <summary>
    /// Specifies interface for Caller Context filter
    /// </summary>
    public interface ICallerContextFilter : ICloneable
    {
        /// <summary>
        /// Specifies MDMSource filter
        /// </summary>
        Collection<EventSource> MDMSourceList { get; set; }

        /// <summary>
        /// Specifies MDMSubscriber filter
        /// </summary>
        Collection<EventSubscriber> MDMSubscriberList { get; set; }

        /// <summary>
        /// Specifies MDMPublisher filter
        /// </summary>
        Collection<MDMPublisher> MDMPublisherList { get; set; }

        /// <summary>
        /// Specifies Application filter
        /// </summary>
        Collection<MDMCenterApplication> ApplicationList { get; set; }

        /// <summary>
        /// Specifies Server Id filter
        /// </summary>
        Collection<Int32> ServerIdList { get; set; }

        /// <summary>
        /// Specifies Server Name filter
        /// </summary>
        Collection<String> ServerNameList { get; set; }

        /// <summary>
        /// Specifies Profile Id filter
        /// </summary>
        Collection<Int32> ProfileIdList { get; set; }

        /// <summary>
        /// Specifies Profile Name filter
        /// </summary>
        Collection<String> ProfileNameList { get; set; }

        /// <summary>
        /// Specifies Module filter
        /// </summary>
        Collection<MDMCenterModules> ModuleList { get; set; }

        /// <summary>
        /// Specifies Program Name filter
        /// </summary>
        Collection<String> ProgramNameList { get; set; }

        /// <summary>
        /// Specifies Job Id filter
        /// </summary>
        Collection<Int64> JobIdList { get; set; }

        /// <summary>
        /// Specifies Activity Id filter
        /// </summary>
        Collection<Guid> ActivityIdList { get; set; }

        /// <summary>
        /// Specifies Activity Name filter
        /// </summary>
        Collection<String> ActivityNameList { get; set; }

        /// <summary>
        /// Specifies Operation Id filter
        /// </summary>
        Collection<Guid> OperationIdList { get; set; }

        /// <summary>
        /// Adds <see cref="CallerContext"/> values into filter collections
        /// </summary>
        /// <param name="callerContext"><see cref="CallerContext"/> instance which data should be added into filter collections</param>
        void AddCallerContextData(CallerContext callerContext);

        /// <summary>
        /// Returns <see cref="CallerContextFilter"/> in Xml format
        /// </summary>
        /// <returns>String representation of current <see cref="CallerContextFilter"/></returns>
        String ToXml();
    }
}