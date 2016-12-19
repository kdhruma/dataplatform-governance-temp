using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get configuration for MDM trace items.
    /// </summary>
    public interface IMDMTraceConfigItem
    {
        #region Properties

        /// <summary>
        /// Property specifying trace source
        /// </summary>
        MDMTraceSource TraceSource { get; set; }

        /// <summary>
        /// Property specifying whether to consider activity tracing 
        /// </summary>
        Boolean LogActivityTrace { get; set; }

        /// <summary>
        /// Property specifying whether to consider error logging 
        /// </summary>
        Boolean LogError { get; set; }

        /// <summary>
        /// Property specifying whether to consider warning logging 
        /// </summary>
        Boolean LogWarning { get; set; }

        /// <summary>
        /// Property specifying whether to consider information logging 
        /// </summary>
        Boolean LogInformation { get; set; }

        /// <summary>
        /// Property specifying whether to consider verbose logging
        /// </summary>
        Boolean LogVerbose { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of MDM Trace Config Item
        /// </summary>
        /// <returns>Xml representation of MDM Trace Config Item</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of MDM Trace Config Item based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of MDM Trace Config Item</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
