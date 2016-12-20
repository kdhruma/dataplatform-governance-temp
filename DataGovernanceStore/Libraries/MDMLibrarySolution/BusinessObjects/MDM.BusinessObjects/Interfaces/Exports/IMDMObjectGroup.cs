using System;
using MDM.BusinessObjects.Exports;
using MDM.Core;
using System.Collections;
using System.Collections.ObjectModel;

namespace MDM.Interfaces.Exports
{
    /// <summary>
    /// Exposes methods or properties to set or get the MDM object group related information.
    /// </summary>
    public interface IMDMObjectGroup : IMDMObject
    {
        #region Properties

        /// <summary>
        /// Property specifying object type of mdmobjectgroup
        /// </summary>
        ObjectType ObjectType { get; set; }

        /// <summary>
        /// Property specifying include all mdmobject or not
        /// </summary>
        Boolean IncludeAll { get; set; }

        /// <summary>
        /// Property specifying include empty mdmobject or not
        /// </summary>
        Boolean IncludeEmpty { get; set; }

        /// <summary>
        /// Property specifying start with all mdmobject or not
        /// </summary>
        Boolean StartWith { get; set; }

        /// <summary>
        /// Property specifying collection of mdmObject
        /// </summary>
        MDMObjectCollection MDMObjects { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList GetMDMObjectNames();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList GetMDMObjectIdList();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Collection<Int32> GetMDMObjectIds();

        /// <summary>
        /// Represents mdmobjectgroup in Xml format
        /// </summary>
        /// <returns>mdmobjectgroup in Xml format</returns>
        String ToXml();

        /// <summary>
        /// Represents mdmobjectgroup in Xml format based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of mdmobjectgroup</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion Methods
    }
}
