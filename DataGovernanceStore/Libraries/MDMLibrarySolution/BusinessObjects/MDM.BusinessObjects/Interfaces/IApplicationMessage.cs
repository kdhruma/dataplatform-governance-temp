using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the application message object.
    /// </summary>
    public interface IApplicationMessage : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property denoting code of the message
        /// </summary>
        String Code { get; set; }

        /// <summary>
        /// Property denoting type of the message
        /// </summary>
        String Type { get; set; }

        /// <summary>
        /// Property denoting type of the message
        /// </summary>
        String Message { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
