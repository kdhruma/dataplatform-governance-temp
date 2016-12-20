using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity validation score instance.
    /// </summary>
    public interface IEntityStateValidationScore : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates id of the entity for which validation score is calculated
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Indicates over all entity validation score
        /// </summary>
        Double OverallScore { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get XMl representation of entity validation score
        /// </summary>
        /// <returns>XMl string representation of entity validation score</returns>
        String ToXml();

        /// <summary>
        /// Get EntityStateValidationAttributeScore collection
        /// </summary>
        /// <returns>Returns EntityStateValidationAttributeScoreCollection interface</returns>
        IEntityStateValidationAttributeScoreCollection GetEntityStateValidationAttributeScores();

        /// <summary>
        /// Set EntityStateValidationAttributeScore collection
        /// </summary>
        /// <param name="iEntityStateValidationAttributeScores">Indicates collection object to be set</param>
        /// <exception cref="ArgumentNullException">Raised when passed EntityStateValidationAttributeScoreCollection is null</exception>
        void SetEntityStateValidationAttributeScores(IEntityStateValidationAttributeScoreCollection iEntityStateValidationAttributeScores);

        #endregion Methods
    }
}