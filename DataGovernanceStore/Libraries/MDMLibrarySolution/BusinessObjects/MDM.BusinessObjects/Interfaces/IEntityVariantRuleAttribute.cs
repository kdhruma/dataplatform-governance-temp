using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity variant rule attribute.
    /// </summary>
    public interface IEntityVariantRuleAttribute : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting if rule attribute is optional
        /// </summary>
        Boolean IsOptional { get; set; }
        
        /// <summary>
        /// Property denoting target attribute Id
        /// </summary>
        Int32 TargetAttribute { get; set; }

        /// <summary>
        /// Property denoting source attribute identifier
        /// </summary>
        Int32 SourceAttributeId { get; set; }

        /// <summary>
        /// Property denoting source attribute name
        /// </summary>
        String SourceAttributeName { get; set; }

        /// <summary>
        /// Property denoting source attribute long name
        /// </summary>
        String SourceAttributeLongName { get; set; }

        /// <summary>
        /// Property denoting target attribute identifier
        /// </summary>
        Int32 TargetAttributeId { get; set; }

        /// <summary>
        /// Property denoting target attribute name
        /// </summary>
        String TargetAttributeName { get; set; }

        /// <summary>
        /// Property denoting target attribute long name
        /// </summary>
        String TargetAttributeLongName { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get rule attribute
        /// </summary>
        /// <returns>Returns Rule attribute interface</returns>
        IAttribute GetRuleAttribute();

        /// <summary>
        /// Set rule attribute
        /// </summary>
        /// <param name="ruleAttribute">Indicates rule attribute interface to set</param>
        void SetRuleAttribute(IAttribute ruleAttribute);

        #endregion Methods
    }
}
