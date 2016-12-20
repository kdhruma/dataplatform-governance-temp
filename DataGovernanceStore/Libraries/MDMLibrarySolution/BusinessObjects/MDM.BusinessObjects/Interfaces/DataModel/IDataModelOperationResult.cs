using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MDM.BusinessObjects.DQM;

namespace MDM.Interfaces
{
    using MDM.Core;
    using MDM.BusinessObjects;

    /// <summary>
    /// Exposes methods or properties to set or get data model operation result.
    /// </summary>
    public interface IDataModelOperationResult : IOperationResult
    {
        #region Properties

        /// <summary>
        /// Property denoting the long name of the entity for which results are created
        /// </summary>
        String LongName { get; set; }

        /// <summary>
        /// DataModel ObjectType
        /// </summary>
        ObjectType DataModelObjectType { get; set; }

        /// <summary>
        /// PerformedAction
        /// </summary>
        new ObjectAction PerformedAction { get; set; }
  
        #endregion

        #region Methods

        /// <summary>
        /// Get Xml representation of DataModel Operation Result
        /// </summary>
        /// <returns>Xml representation of DataModel Operation Result object</returns>
        new String ToXml();

        /// <summary>
        /// Get Xml representation of DataModel operation result based on requested object serialization
        /// </summary>
        /// <param name="objectSerialization">Type of the serialization option</param>
        /// <returns>Xml representation of DataModel operation result</returns>
        String ToXml(ObjectSerialization objectSerialization);

        #endregion
    }
}
