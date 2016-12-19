using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get warning related information.
    /// </summary>
    public interface IWarning
    {
        #region Properties

        /// <summary>
        /// Field denoting Warning code
        /// </summary>
        String WarningCode { get; set; }

        /// <summary>
        /// Field denoting Warning message
        /// </summary>
        String WarningMessage { get; set; }

        /// <summary>
        /// Field denoting additional parameters for Warning 
        /// </summary>
        Collection<Object> Params { get; set; }

        /// <summary>
        /// Field denoting the Reason type
        /// </summary>
        ReasonType ReasonType { get; set; }

        /// <summary>
        /// Field denoting the unique identifier for rule map context  
        /// </summary>
        Int32 RuleMapContextId { get; set; }

        /// <summary>
        /// Field denoting the id of rule
        /// </summary>
        Int32 RuleId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Warning
        /// </summary>
        /// <returns>Xml representation of Warning object</returns>
        String ToXml();

        #endregion
    }
}