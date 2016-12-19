using System;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the execution specification business object.
    /// </summary>
    public interface IExecutionSpecification : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying executionsettingcollection object
        /// </summary>
        ExecutionSettingCollection ExecutionSettings { get; set; }

   
        #endregion Properties

        #region Methods

        /// <summary>
        /// Represents execution specification in Xml format
        /// </summary>
        /// <returns>execution specification in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents execution specification in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of execution specification</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
