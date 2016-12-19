using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace MDM.JigsawIntegrationManager.DTO
{
    /// <summary>
    /// Represents class for JigsawEntity.
    /// </summary>
    /// <seealso cref="MDM.JigsawIntegrationManager.JigsawHelpers.IJigsawJsonSerializable" />
    internal abstract class MessageBase : IMessageBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets the eid.
        /// </summary>
        public String Eid { get; set; }

        /// <summary>
        /// Gets or sets the entity information.
        /// </summary>
        public EntityInfo EntityInfo { get; set; }

        /// <summary>
        /// Gets or sets the system information.
        /// </summary>
        public SystemInfo SystemInfo { get; set; }

        /// <summary>
        /// Gets or sets the attributes information.
        /// </summary>
        public AttributesInfo AttributesInfo { get; set; }

        /// <summary>
        /// Gets or sets the invalid attributes information.
        /// </summary>
        public AttributesInfo InvalidAttributesInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IExtendedAttributesInfo ExtendedAttributesInfo { get; set; }
        
        #endregion

    }
}