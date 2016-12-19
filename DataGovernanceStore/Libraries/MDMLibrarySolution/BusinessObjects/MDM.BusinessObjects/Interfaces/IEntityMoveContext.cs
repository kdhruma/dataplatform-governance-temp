using System;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity move context when entity is being reclassified.
    /// </summary>
    public interface IEntityMoveContext
    {
        #region Properties

        /// <summary>
        /// Indicates ReParent
        /// </summary>
        ReParentTypeEnum ReParentType { get; set; }

        /// <summary>
        /// Indicates target category id
        /// </summary>
        Int64 TargetCategoryId { get; set; }

        /// <summary>
        /// Indicates from Category id
        /// </summary>
        Int64 FromCategoryId { get; set; }

        /// <summary>
        /// Indicates target category name
        /// </summary>
        String TargetCategoryName { get; set; }

        /// <summary>
        /// Indicates target category path
        /// </summary>
        String TargetCategoryPath { get; set; }

        /// <summary>
        /// Indicates target parent entity id.
        /// </summary>
        Int64 TargetParentEntityId { get; set; }

        /// <summary>
        /// Indicates target parent extension entity id.
        /// </summary>
        Int64 TargetParentExtensionEntityId { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents EntityMoveContext in Xml format
        /// </summary>
        /// <returns>String representing EntityMoveContext Xml</returns>
        String ToXml();

        #endregion Methods
    }
}
