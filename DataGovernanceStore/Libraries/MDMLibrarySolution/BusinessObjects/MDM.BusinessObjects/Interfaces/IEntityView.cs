using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get entity view details.
    /// </summary>
    public interface IEntityView
    {
        #region Properties

        /// <summary>
        /// Property which uniquely identifies the entity view
        /// </summary>
        String UniqueIdentifier { get; set; }

        /// <summary>
        /// Property denoting the list of attribute ids configured for the view
        /// </summary>
        Collection<Int32> AttributeIdList { get; set; }

        /// <summary>
        /// Property denoting the completion criterion for the view
        /// </summary>
        CompletionCriterionEnum CompletionCriterion { get; set; }

        /// <summary>
        /// Property denoting the completion status for the view
        /// </summary>
        Boolean CompletionStatus { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Entity View
        /// </summary>
        /// <returns>Xml representation of Entity View</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of Entity View based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Entity View</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
