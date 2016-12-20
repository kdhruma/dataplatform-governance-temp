using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace MDM.InstrumentationManager.Utility
{
    /// <summary>
    /// Represents the data model metadata 
    /// </summary>
    [DataContract(Namespace = "http://mdmobjects.riversand.com/")]
    public enum DiagnosticObjectType
    {
        /// <summary>
        /// Represents the unknow/uninitialized type
        /// </summary>
        [EnumMember]
        Unnown,

        /// <summary>
        /// Represents the DiagnosticActivity
        /// </summary>
        [EnumMember]
        DiagnosticActivity,

        /// <summary>
        /// Represents the DiagnosticRecord
        /// </summary>
        [EnumMember]
        DiagnosticRecord
    }
}