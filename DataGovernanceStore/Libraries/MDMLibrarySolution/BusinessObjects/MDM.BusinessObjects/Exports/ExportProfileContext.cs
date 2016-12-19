using System;
using System.Runtime.Serialization;
using MDM.Interfaces.Exports;

namespace MDM.BusinessObjects.Exports
{
    /// <summary>
    /// ExportProfileContext
    /// </summary>
    [DataContract]
    public class ExportProfileContext : IExportProfileContext
    {
        /// <summary>
        /// Property denoting Id of the export profile
        /// </summary>
        [DataMember]
        public Int32 ProfileId { get; set; }

        /// <summary>
        /// Property denoting short name of the export profile
        /// </summary>
        [DataMember]
        public String ShortName { get; set; }
    }
}
