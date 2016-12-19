using System;
using System.Collections.Generic;

namespace MDM.Interfaces
{
    using MDM.BusinessObjects.Workflow;
using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get the collection of tracked activity information.
    /// </summary>
    public interface ITrackedActivityInfoCollection : IEnumerable<TrackedActivityInfo>
    {
        #region Properties

        #endregion

        #region Methods

        #region ToXml Methods

        /// <summary>
        /// Get Xml representation of TrackedActivityInfoCollection object
        /// </summary>
        /// <returns>Xml string representing the TrackedActivityInfoCollection</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of the object
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <param name="activityDetailOnly">If it is false then return object and collection details else only object detail </param>
        /// <returns>Xml representation of the object</returns>
        String ToXml(ObjectSerialization objectSerialization, bool activityDetailOnly);

        #endregion

        #endregion

    }
}
