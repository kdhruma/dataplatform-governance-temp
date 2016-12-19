using System;
using MDM.Core;

namespace MDM.Interfaces
{

    /// <summary>
    /// Exposes methods or properties to set or get server information.
    /// </summary>
    public interface IServerInfo : IMDMObject
    {
        #region Properties


        #endregion

        #region Methods

        /// <summary>
        /// Represents IServerInfo  in Xml format
        /// </summary>
        /// <returns>
        /// IServerInfo  in Xml format
        /// </returns>
        String ToXml();

        #endregion
    }
}
