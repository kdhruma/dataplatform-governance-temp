using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Exposes methods or properties used for representing an error.
    /// </summary>
    public interface IError
    {
        #region Properties

        /// <summary>
        /// Field denoting code of the error.
        /// </summary>
        String ErrorCode { get; set; }

        /// <summary>
        /// Field denoting actual error message
        /// </summary>
        String ErrorMessage { get; set; }

        /// <summary>
        /// Field denoting additional parameters for error 
        /// </summary>
        Collection<Object> Params { get; set; }

        /// <summary>
        /// Field denoting the Reference Id
        /// </summary>
        String ReferenceId { get; set; }

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
        /// Get Xml representation of Error
        /// </summary>
        /// <returns>Xml representation of Error object</returns>
        String ToXml();

        #endregion
    }
}
