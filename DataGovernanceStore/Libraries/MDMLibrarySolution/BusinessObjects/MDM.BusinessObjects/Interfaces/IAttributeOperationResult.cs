using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get attribute operation result.
    /// </summary>
    public interface IAttributeOperationResult : IOperationResult
    {
        #region Properties

        /// <summary>
        /// Property denoting the id of the attribute for which results are created
        /// </summary>
        Int32 AttributeId { get; set; }

        /// <summary>
        /// Property denoting the short name of the attribute for which results are created
        /// </summary>
        String AttributeShortName { get; set; }

        /// <summary>
        /// Property denoting the long name of the attribute for which results are created
        /// </summary>
        String AttributeLongName { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of Attribute Operation Result
        /// </summary>
        /// <returns>Xml representation of Attribute Operation Result object</returns>
        new String ToXml();

        /// <summary>
        /// Get Xml representation of Attribute operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of Attribute operation result</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
