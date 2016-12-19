using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods used for setting configuration for MDM trace.
    /// </summary>
    public interface IMDMTraceConfig
    {
        #region Methods

        #region ToXml methods

        /// <summary>
        /// Get Xml representation of MDM Trace Config
        /// </summary>
        /// <returns>Xml representation of MDM Trace Config</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of MDM Trace Config based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of MDM Trace Config</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion

        #region MDM Trace Config Items methods

        /// <summary>
        /// Gets MDM Trace Config Items
        /// </summary>
        /// <returns>MDM Trace Config Items</returns>
        Collection<IMDMTraceConfigItem> GetMDMTraceConfigItems();

        /// <summary>
        /// Sets MDM Trace Config Items
        /// </summary>
        /// <param name="iMDMTraceConfigItems">MDM Trace Config Items to be set</param>
        /// <exception cref="ArgumentNullException">Raised when passed hierarchy relationship collection is null</exception>
        void SetMDMTraceConfigItems(Collection<IMDMTraceConfigItem> iMDMTraceConfigItems);

        #endregion

        #endregion
    }
}
