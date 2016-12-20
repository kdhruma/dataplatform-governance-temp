using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDM.BusinessObjects;

namespace MDM.Interfaces
{
    /// <summary>
    /// Exposes methods or properties to set or get the context for matching profile group.
    /// </summary>
    public interface IProfileGroupContext
    {
        #region Fields

        /// <summary>
        /// Indicates the Id of the Profile Group Context
        /// </summary>
        Int32 Id { get; set; }

        /// <summary>
        /// Indicates Application Context for Profile Group
        /// </summary>
        ApplicationContext ApplicationContext { get; set; }

        /// <summary>
        /// Indicates Matching Profile Id
        /// </summary>
        Int32 ProfileId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of ProfileGroupContext object
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        #endregion
    }
}
