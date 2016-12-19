using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.ParallelizationManager.Interfaces;

    /// <summary>
    /// Represents class for Mdmmessage package
    /// </summary>    
    [DataContract]
    public class MDMMessagePackage : ObjectBase, IMDMMessagePackage
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private Object _data;

        /// <summary>
        /// 
        /// </summary>
        private String _extendedProperties;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Object Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ExtendedProperties
        {
            get
            {
                return _extendedProperties;
            }
            set
            {
                _extendedProperties = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MDMMessagePackage()
        {
        }

        /// <summary>
        /// Constructor accepting data and data result
        /// </summary>
        /// <param name="data"></param>
        public MDMMessagePackage(Object data)
        {
            this._data = data;
        }
        
        #endregion

        #region Methods

        #endregion
    }
}
