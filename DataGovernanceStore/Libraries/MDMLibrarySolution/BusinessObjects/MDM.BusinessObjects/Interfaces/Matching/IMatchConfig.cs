using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods used for getting adapter configuration and its connection information.
    /// </summary>
    public interface IMatchConfig
    {
        /// <summary>
        /// Gets the value for the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returns the value. </returns>
        string GetConfigValue(string key);
    }
}
