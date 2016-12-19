using System;

namespace MDM.Interfaces.Diagnostics
{
    using MDM.Core;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Exposes methods or properties to set or get the diagnostic record related information.
    /// </summary>
    public interface IDiagnosticRecord : IDiagnosticDataElement
    {
        #region Properties

        /// <summary>
        ///
        /// </summary>
        String MessageCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        MessageClassEnum MessageClass { get; set; }

        /// <summary>
        ///
        /// </summary>
        String Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Collection<String> MessageParameters { get; set; }

        /// <summary>
        ///
        /// </summary>
        String DataXml { get; set; }

        /// <summary>
        /// Specifies Extended Data existence status on DB level. Used for lazy loading purposes only.
        /// </summary>
        Boolean HasExtendedDataInDB { get; }

        /// <summary>
        /// 
        /// </summary>
        Double DurationInMilliSeconds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 ThreadId { get; }

        /// <summary>
        /// 
        /// </summary>
        Int32 ThreadNumber { get; }

        #endregion

        #region Methods

        #endregion
    }
}
