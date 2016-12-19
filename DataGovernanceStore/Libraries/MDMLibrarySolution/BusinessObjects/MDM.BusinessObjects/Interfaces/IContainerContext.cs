using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties used for specifying container context, which indicates information to be loaded into the container object.
    /// </summary>
    public interface IContainerContext
    {
        #region Properties

        /// <summary>
        /// Property denoting whether Attributes are to be loaded or not
        /// </summary>
        Boolean LoadAttributes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Represents ContainerContext  in Xml format
        /// </summary>
        /// <returns>ContainerContext  in Xml format</returns>
        String ToXml();

        #endregion

    }
}
