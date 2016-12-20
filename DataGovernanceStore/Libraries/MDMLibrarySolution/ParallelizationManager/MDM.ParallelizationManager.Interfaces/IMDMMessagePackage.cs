using System;
using System.Runtime.Serialization;

namespace MDM.ParallelizationManager.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMDMMessagePackage
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        Object Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        String ExtendedProperties { get; set; }

        /// <summary>
        /// Property defining the type id of the MDM object
        /// </summary>
        Int32 ObjectTypeId { get; }

        /// <summary>
        /// Property defining the type of the MDM object
        /// </summary>
        String ObjectType { get; }
    }
}