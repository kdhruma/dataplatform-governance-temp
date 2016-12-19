using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    /// <summary>
    /// Obsolete. Exposes properties used for identifying entity type uniquely.
    /// </summary>
    [System.Obsolete("EntityTypeUniqueIdentifier is no longer needed as EntityType short name is enough to identify entityType uniquely.")]    
    public interface IEntityTypeUniqueIdentifier
    {
        #region Properties

        /// <summary>
        /// Property denoting EntityType short name
        /// </summary>
        String EntityTypeName { get; set; }

        /// <summary>
        /// Property denoting EntityType group short name
        /// </summary>
        String ParentEntityTypeName { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
