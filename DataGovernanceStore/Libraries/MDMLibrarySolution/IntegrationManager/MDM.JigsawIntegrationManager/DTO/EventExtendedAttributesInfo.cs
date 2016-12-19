using System;
using System.Collections.Generic;

namespace MDM.JigsawIntegrationManager.DTO
{
    /// <summary>
    /// Represents class for EventExtendedAttributesInfo
    /// </summary>
    /// <seealso cref="MDM.JigsawIntegrationManager.JigsawHelpers.IJigsawJsonSerializable" />
    internal class EventExtendedAttributesInfo : IExtendedAttributesInfo
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the js relationship.
        /// </summary>
        public Relationship JsRelationship { get; set; }

        /// <summary>
        /// Gets or sets the change context.
        /// </summary>
        public ChangeContext JsChangeContext { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="JigsawAttributesInfo" /> class.
        /// </summary>
        public EventExtendedAttributesInfo()
        {
        }
    }
}