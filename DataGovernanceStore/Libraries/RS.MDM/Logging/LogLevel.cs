using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace RS.MDM.Logging
{
    /// <summary>
    /// Defines the default set of levels recognized by the system for Logging. 
    /// </summary>
    [DataContract(Namespace = "http://Riversand.MDM.Services")]
    public enum LogLevel
    {
        /// <summary>
        /// The All level designates the lowest level possible.
        /// </summary>
        [EnumMember]
        ALL,

        /// <summary>
        /// The Debug level designates fine-grained informational messages that are most useful to debug the application.
        /// </summary>
        [EnumMember]
        DEBUG,

        /// <summary>
        /// The Info level designates informational messages that highlight the progress of the application at coarse-grained level.
        /// </summary>
        [EnumMember]
        INFO,

        /// <summary>
        /// The Warn level designates potentially harmful situations.
        /// </summary>
        [EnumMember]
        WARN,

        /// <summary>
        /// The Error level designates error events that might still allow the application to continue running.
        /// </summary>
        [EnumMember]
        ERROR,

        /// <summary>
        /// The Fatal level designates very severe error events that will presumably lead the application to abort.
        /// </summary>
        [EnumMember]
        FATAL,

        /// <summary>
        /// The Off level designates a higher level than all the rest.
        /// </summary>
        [EnumMember]
        OFF

    }
}
