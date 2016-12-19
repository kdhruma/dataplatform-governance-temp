using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties used for providing entity state validation collection.
    /// </summary>
    public interface IEntityStateValidationCollection : ICollection<EntityStateValidation>
    {
        #region Properties
        #endregion

        #region Methods

        /// <summary>
        /// Gets Xml representation of EntityStateValidationCollection object        
        /// </summary>   
        /// <returns>Returns Xml representation of EntityStateValidationCollection object</returns>
        String ToXml();

        #endregion
    }
}