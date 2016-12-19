using System;
using System.Collections.ObjectModel;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get business rule operation result.
    /// </summary>
    public interface IBusinessRuleOperationResult : IOperationResult
    {
        #region Properties

        /// <summary>
        /// Property denotes the Reference Id
        /// </summary>
        new Int64 ReferenceId { get; set; }

        /// <summary>
        /// Property denotes the identifier of the MDMRule
        /// </summary>
        Int32 RuleId { get; set; }

        /// <summary>
        /// Property denotes the name of the MDMRule
        /// </summary>
        String RuleName { get; set; }

        /// <summary>
        /// Property denotes the identifier of the MDMRule Map
        /// </summary>
        Int32 RuleMapId { get; set; }

        /// <summary>
        /// Property denotes the MDMRule type
        /// </summary>
        MDMRuleType RuleType { get; set; }

        /// <summary>
        /// Property denotes the DDG locale message code
        /// </summary>
        String DDGLocaleMessageCode { get; set; }

        /// <summary>
        /// Property denotes the DDG locale message id
        /// </summary>
        Int32 DDGLocaleMessageId { get; set; }

        /// <summary>
        /// Property denotes the DDG locale message locale id
        /// </summary>
        LocaleEnum Locale { get; set; }

        #endregion Properties
    }
}
