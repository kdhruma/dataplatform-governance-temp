using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    /// <summary>
    /// Interface for MMD Object base class
    /// </summary>
    public interface IObject
    {
        /// <summary>
        /// Gets the XML representation of current instance
        /// </summary>
        /// <returns>XML string representing current instance of Ojbect</returns>
        String ToXml();
    }
}
