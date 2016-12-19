using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    /// <summary>
    /// Interface for the strongly typed entity classes
    /// </summary>
    public interface IStronglyTypedEntity
    {
        /// <summary>
        /// Gets the attribute ids to load.
        /// </summary>
        /// <returns>Attribute ids to load</returns>
        ICollection<Int32> GetAttributeIdsToLoad();
    }
}
