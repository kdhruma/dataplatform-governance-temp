using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Exposes methods or properties to set or get the information object.
    /// </summary>
    public interface IInformation 
    {
        #region Properties

        /// <summary>
        /// Field denoting information code
        /// </summary>
        String InformationCode { get; set; }

        /// <summary>
        /// Field denoting information message
        /// </summary>
        String InformationMessage { get; set; }

        /// <summary>
        /// Field denoting additional parameters for information 
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
        /// Get Xml representation of Information
        /// </summary>
        /// <returns>Xml representation of Information object</returns>
        String ToXml();

        #endregion
    }
}
