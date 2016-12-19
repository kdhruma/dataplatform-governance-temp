using System;

namespace MDM.Interfaces
{
    using MDM.Core;
    
    /// <summary>
    /// Exposes methods or properties to set or get entity business condition instance.
    /// </summary>
    public interface IEntityBusinessCondition : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Indicates id of the entity for which business conditions are calculated
        /// </summary>
        Int64 EntityId { get; set; }

        /// <summary>
        /// Indicates entity family id for a variant tree
        /// </summary>
        Int64 EntityFamilyId { get; set; }

        /// <summary>
        /// Indicates entity global family id across parent(including extended families)
        /// </summary>
        Int64 EntityGlobalFamilyId { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get XMl representation of entity business condition
        /// </summary>
        /// <returns>XMl string representation of entity business condition</returns>
        String ToXml();

        /// <summary>
        /// Get BusinessCondition collection
        /// </summary>
        /// <returns>Returns BusinessConditionCollection interface</returns>
        IBusinessConditionStatusCollection GetBusinessConditions();

        /// <summary>
        /// Set BusinessCondition collection
        /// </summary>
        /// <param name="iBusinessConditions">Indicates collection object to be set</param>
        /// <exception cref="ArgumentNullException">Raised when passed BusinessConditionCollection is null</exception>
        void SetBusinessConditions(IBusinessConditionStatusCollection iBusinessConditions);

        #endregion Methods
    }
}
