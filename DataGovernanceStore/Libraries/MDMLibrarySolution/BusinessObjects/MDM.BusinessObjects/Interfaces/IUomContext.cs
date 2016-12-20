using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the UOM context as container context, which indicates information to be loaded into the container.
    /// </summary>
    public interface IUomContext
    {
        #region Properties

        /// <summary>
        /// Specifies uom id
        /// </summary>
        Int32 UomId { get; set; }

        ///<summary>
        /// Specifies uom short name
        ///</summary>
        String UomShortName { get; set; }

        ///<summary>
        /// Specifies uom type
        ///</summary>
        String UomType { get; set; }

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
