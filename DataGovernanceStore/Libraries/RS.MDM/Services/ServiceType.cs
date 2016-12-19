using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace RS.MDM.Services
{
    /// <summary>
    /// Specifies the Type of a Service within MDM Framework
    /// </summary>
    [DataContract(Namespace = "http://Riversand.MDM.Services")]
    public class ServiceType : RS.MDM.Object
    {

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public ServiceType()
            : base()
        {
        }

        /// <summary>
        /// Constructor with Id as input parameter
        /// </summary>
        /// <param name="id"></param>
        public ServiceType(int id)
            : base(id)
        {
        }

        /// <summary>
        /// Constructor with Id, Name and Descriptions as input parameters
        /// </summary>
        /// <param name="id">Indicates the identity of the Service Type</param>
        /// <param name="name">Indicates the Name of the Service Type</param>
        /// <param name="description">Indicates the Description of the Service Type</param>
        public ServiceType(int id, string name, string description)
            : base(id, name, description)
        {
        }

        #endregion

    }
}
