using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Interfaces;
    using System.Runtime.Serialization;
    
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class DataObjectOperationRequest : IDataObjectOperationRequest
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private Boolean _returnRequest = false;

        /// <summary>
        /// 
        /// </summary>
        private List<DataObject> _dataObjects = null;

        ///// <summary>
        ///// 
        ///// </summary>
        //private List<JObject> _dataObjects = null;

        /// <summary>
        /// 
        /// </summary>
        private String _requestParams = null;

        /// <summary>
        /// 
        /// </summary>
        private String _serviceName = null;

        /// <summary>
        /// 
        /// </summary>
        private String _requestId = null;

        #endregion Fields

        #region Constructors
        #endregion Constructors

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Boolean ReturnRequest
        {
            get
            {
                return this._returnRequest;
            }
            set
            {
                this._returnRequest = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<DataObject> DataObjects
        {
            get
            {
                if (this._dataObjects == null)
                {
                    this._dataObjects = new List<DataObject>();
                }

                return this._dataObjects;
            }
            set
            {
                this._dataObjects = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String RequestParams
        {
            get
            {
                if (this._requestParams == null)
                {
                    this._requestParams = String.Empty;
                }

                return this._requestParams;
            }
            set
            {
                this._requestParams = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ServiceName
        {
            get
            {
                return this._serviceName;
            }
            set
            {
                this._serviceName = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String RequestId
        {
            get
            {
                return this._requestId;
            }
            set
            {
                this._requestId = value;
            }
        }

        #endregion Properties

        #region Methods

        #region Public Methods
        #endregion Public Methods

        #region Private Methods
        #endregion Private Methods

        #endregion Methods
    }
}
