using System;
using System.Collections.Generic;
using System.Linq;

namespace MDM.Interfaces
{
    using MDM.Core;

    /// <summary>
    /// Exposes methods or properties to set or get job parameters.
    /// </summary>
    public interface IJobParameter
    {
        #region Properties

        /// <summary>
        /// Represents the name of the parameter
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Represents the value of the parameter
        /// </summary>
        String Value { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <returns>Xml representation of object</returns>
        String ToXml();

        /// <summary>
        /// Get Xml representation of JobData
        /// </summary>
        /// <param name="serialization">Type of Object Serialization</param>
        /// <returns>Xml representation of object</returns>
        String ToXml(ObjectSerialization serialization);

        #endregion
    }
}
