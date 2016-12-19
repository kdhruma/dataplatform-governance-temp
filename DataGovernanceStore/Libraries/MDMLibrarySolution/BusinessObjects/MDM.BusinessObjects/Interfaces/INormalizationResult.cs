using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get normalization result related information.
    /// </summary>
    public interface INormalizationResult : IMDMObject
    {
        /// <summary>
        /// Property denoting id of job which create result
        /// </summary>
        new Int64 Id { get; set; }

        /// <summary>
        /// Property denoting id of job which create result
        /// </summary>
        Int64 JobId { get; set; }

        /// <summary>
        /// Property denoting attribute Id which was changed
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Property denoting Cnode Id which was changed
        /// </summary>
        Int64 CnodeId { get; set; }

        /// <summary>
        /// Property denoting DateTime when attribute was changed. Please set to Null if you want to use current SQL server time as ChangeDateTime during saving operations.
        /// </summary>
        DateTime? ChangeDateTime { get; set; }

        /// <summary>
        /// Property denoting rule id which was applied
        /// </summary>
        Int32 RuleId { get; set; }

        /// <summary>
        /// Property denoting attribute value which was before changing
        /// </summary>
        String OldAttributeValue { get; set; }

        /// <summary>
        /// Property denoting attribute value which was created after changing
        /// </summary>
        String NewAttributeValue { get; set; }

        /// <summary>
        /// Property denoting if attribute was changed successfully
        /// </summary>
        Boolean IsNormalizationSucceeded { get; set; }

        /// <summary>
        /// Property denoting result message
        /// </summary>
        String ResultMessage { get; set; }
    }
}