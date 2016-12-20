using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get attribute unique identifier instance.
    /// </summary>
    public interface IAttributeUniqueIdentifier
    {
        #region Properties

        /// <summary>
        /// Property denoting attribute short name
        /// </summary>
        String AttributeName { get; set; }

        /// <summary>
        /// Property denoting attribute group short name
        /// </summary>
        String AttributeGroupName { get; set; }

        /// <summary>
        /// Property denoting instance ref Id for attribute.
        /// This is used for identifying 1 complex child attribute when attribute is complex collection
        /// </summary>
        Int32 InstanceRefId { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
