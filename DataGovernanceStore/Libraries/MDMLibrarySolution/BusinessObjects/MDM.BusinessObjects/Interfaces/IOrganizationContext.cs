using System;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the context which indicates information to be loaded into organization object.
    /// </summary>
    public interface IOrganizationContext
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
