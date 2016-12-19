using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity state validation attribute score instance.
    /// </summary>
    public interface IEntityStateValidationAttributeScore : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates state attribute for the entity
        /// </summary>
        SystemAttributes StateValidationAttribute { get; set; }

        /// <summary>
        /// Indicates maximum score for the state attribute
        /// </summary>
        Int32 Weightage { get; set; }

        /// <summary>
        /// Indicates calculated score for the state attribute
        /// </summary>
        Double Score { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get XMl representation of entity validation score
        /// </summary>
        /// <returns>XMl string representation of entity state validation attribute score</returns>
        String ToXml();

        #endregion Methods
    }
}
