using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods used for specifying entity history details template collection.
    /// </summary>
    public interface IEntityHistoryDetailsTemplateCollection
    {
        #region Properties

        #endregion

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of EntityHistoryDetailsTemplateCollection object
        /// </summary>
        /// <returns>Xml string representing the EntityHistoryDetailsTemplateCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of EntityHistoryDetailsTemplateCollection object
        /// </summary>
        /// <param name="serialization">serialization option. Based on the value selected, the different xml representation will be there</param>
        /// <returns>Xml string representing the EntityHistoryDetailsTemplateCollection</returns>
        String ToXml(ObjectSerialization serialization);

        /// <summary>
        /// Add EntityHistoryDetailsTemplate object in collection
        /// </summary>
        /// <param name="item">IEntityHistoryDetailsTemplate to add in collection</param>
        void Add(IEntityHistoryDetailsTemplate item);

        #endregion

    }
}
