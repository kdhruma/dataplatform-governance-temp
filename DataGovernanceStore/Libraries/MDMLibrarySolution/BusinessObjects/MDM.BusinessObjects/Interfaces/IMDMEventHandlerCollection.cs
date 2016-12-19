using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    /// <summary>
    /// Contains properties and methods to manipulate mdm event handler collection object
    /// </summary>
    public interface IMDMEventHandlerCollection : ICollection<IMDMEventHandler>, ICloneable
    {
        /// <summary>
        /// Get XML representation of mdm event handler collection object
        /// </summary>
        /// <returns>XML representation of mdm event handler collection object</returns>
        String ToXml();

        /// <summary>
        /// Gets a cloned instance of the current mdm event handler collection object
        /// </summary>
        /// <returns>Cloned instance of the current mdm event handler collection object</returns>
        new IMDMEventHandlerCollection Clone();

    }
}
