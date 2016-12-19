using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the export subscriber.
    /// </summary>
    public interface IExportSubscriber : IMDMObject
    {
        /// <summary>
        /// Property denoting collection of configuration parameters for given type of subscriber. 
        /// These values can be different per subscriber type. So it is a key value collection of configuration parameters.
        /// </summary>
        Collection<KeyValuePair<String, String>> ConfigurationParameters { get; set; }

        /// <summary>
        /// Property denoting subscriber type for export
        /// </summary>
        ExportSubscriberType SubscriberType { get; set; }

        /// <summary>
        /// Represents ExportSubscriber in Xml format
        /// </summary>
        /// <returns>ExportSubscriber in Xml format</returns>
        String ToXml();
    }
}
