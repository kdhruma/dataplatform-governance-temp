using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity provider context, which indicates the context for a entity data provider.
    /// </summary>
    public interface IEntityProviderContext
    {
        #region Properties

        /// <summary>
        /// Property denoting EntityProviderContextType
        /// </summary>
        EntityProviderContextType EntityProviderContextType
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting container name
        /// </summary>
        String ContainerName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting Entity Type Name
        /// </summary>
        String EntityTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Property denoting Organization Name
        /// </summary>
        String OrganizationName
        {
            get;
            set;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Represents IEntityProviderContext instance in Xml format
        /// </summary>
        /// <returns>IEntityProviderContext instance in Xml format</returns>
        String ToXml();

        #endregion
    }
}