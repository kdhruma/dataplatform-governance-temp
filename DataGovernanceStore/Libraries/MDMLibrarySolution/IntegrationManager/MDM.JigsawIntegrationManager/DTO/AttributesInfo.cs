using System;
using System.Collections.Generic;

namespace MDM.JigsawIntegrationManager.DTO
{

    /// <summary>
    /// Represents class for AttributesInfo
    /// </summary>
    /// <seealso cref="MDM.JigsawIntegrationManager.JigsawHelpers.IJigsawJsonSerializable" />
    public class AttributesInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        public Int64? ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        public List<Attribute> Attributes { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="JigsawAttributeCollection" /> class.
        /// </summary>
        public AttributesInfo()
        {
            Attributes = new List<Attribute>();
        }
    }
}