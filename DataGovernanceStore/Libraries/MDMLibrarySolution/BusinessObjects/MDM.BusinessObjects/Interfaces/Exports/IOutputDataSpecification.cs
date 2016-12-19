using System;
using MDM.BusinessObjects.Exports;
using MDM.Core;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the output data specification object.
    /// </summary>
    public interface IOutputDataSpecification : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying MDMObjectGroupcollection object
        /// </summary>
        MDMObjectGroupCollection MDMObjectGroups { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        IMDMObjectGroup GetMDMObjectGroup(ObjectType objectType);

        /// <summary>
        /// Represents outputdata specification in Xml format
        /// </summary>
        /// <returns>outputdata specification in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents outputdata specification in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of outputdata specification</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
