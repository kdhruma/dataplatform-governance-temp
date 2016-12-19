using System;

namespace MDM.JigsawIntegrationManager.DTO
{
    
    /// <summary>
    /// Represents class for JigsawEntity.
    /// </summary>
    /// <seealso cref="MDM.JigsawIntegrationManager.JigsawHelpers.IJigsawJsonSerializable" />
    internal class EntityMessage : MessageBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the attributes information.
        /// </summary>
        public new EntityExtendedAttributesInfo ExtendedAttributesInfo { get; set; }

        #endregion

    }
}