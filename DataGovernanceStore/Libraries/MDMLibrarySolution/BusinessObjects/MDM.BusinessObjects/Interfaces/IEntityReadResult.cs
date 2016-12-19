using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    using MDM.Interfaces;

    /// <summary>
    /// Exposes methods or properties to set or get entity get operation result
    /// </summary>
    public interface IEntityReadResult
    {
        #region Methods

        /// <summary>
        /// Get Xml representation of Entity Read Result
        /// </summary>
        /// <returns>Xml representation of Entity Read Result object</returns>
        String ToXml();

        /// <summary>
        /// Gets collection of entity
        /// </summary>
        /// <returns>Collection of entities from the entity read result</returns>
        IEntityCollection GetEntityCollection();

        /// <summary>
        /// Gets collection of entity operation result
        /// </summary>
        /// <returns>Collection of entity operation result from the entity read result</returns>
        IEntityOperationResultCollection GetEntityOperationResultCollection();

        #endregion Methods
    }

}
