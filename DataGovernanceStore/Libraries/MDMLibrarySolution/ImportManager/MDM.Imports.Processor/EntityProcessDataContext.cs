using MDM.BusinessObjects;
using MDM.BusinessObjects.Diagnostics;
using MDM.Core;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MDM.Imports.Processor
{
    /// <summary>
    /// EntityProcessDataContext
    /// </summary>
    public class EntityProcessDataContext
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int64 Start { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int64 End { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 BatchSize { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 NumberOfAttributesThreadPerEntity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Guid ParentActivityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Guid OperationId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 ThreadNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ContainerName;
        
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String OrganizationName;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String EntityTypeName;

        #endregion

    }
}
