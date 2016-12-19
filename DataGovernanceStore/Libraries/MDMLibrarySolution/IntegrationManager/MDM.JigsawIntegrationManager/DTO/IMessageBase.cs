using System;

namespace MDM.JigsawIntegrationManager.DTO
{
    public interface IMessageBase
    {
        /// <summary>
        /// Gets or sets the attributes information.
        /// </summary>
        /// <value>
        /// The attributes information.
        /// </value>
        AttributesInfo AttributesInfo { get; set; }

        /// <summary>
        /// Gets or sets the invalid attributes information.
        /// </summary>
        /// <value>
        /// The invalid attributes information.
        /// </value>
        AttributesInfo InvalidAttributesInfo { get; set; }

        /// <summary>
        /// Gets or sets the entity identity (Guid).
        /// </summary>
        /// <value>
        /// The eid guid.
        /// </value>
        String Eid { get; set; }

        /// <summary>
        /// Gets or sets the entity information.
        /// </summary>
        /// <value>
        /// The entity information.
        /// </value>
        EntityInfo EntityInfo { get; set; }

        /// <summary>
        /// Gets or sets the extended attributes information.
        /// </summary>
        /// <value>
        /// The extended attributes information.
        /// </value>
        IExtendedAttributesInfo ExtendedAttributesInfo { get; set; }

        /// <summary>
        /// Gets or sets the system information.
        /// </summary>
        /// <value>
        /// The system information.
        /// </value>
        SystemInfo SystemInfo { get; set; }
    }
}
