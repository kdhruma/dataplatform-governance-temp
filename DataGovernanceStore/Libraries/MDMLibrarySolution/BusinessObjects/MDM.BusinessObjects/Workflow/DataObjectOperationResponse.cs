using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace MDM.BusinessObjects.Workflow
{
    using MDM.Interfaces;
    
    /// <summary>
    /// 
    /// </summary>
    public class DataObjectOperationResponse : IDataObjectOperationResponse
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private String _status = null;
        
        /// <summary>
        /// 
        /// </summary>
        private List<DataObject> _dataObjects = null;
        
        /// <summary>
        /// 
        /// </summary>
        private String _responseParams = null;
        
        /// <summary>
        /// 
        /// </summary>
        private String _error = null;

        #endregion Fields

        #region Constructors
        #endregion Constructors

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public String Status 
        {
            get
            {
                return this._status;
            }
            set 
            {
                this._status = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<DataObject> DataObjects 
        { 
            get
            {
                if(this._dataObjects == null)
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
        public String ResponseParams 
        { 
            get
            {
                if(this._responseParams == null)
                {
                    this._responseParams = String.Empty;
                }

                return this._responseParams;
            }
            set
            {
                this._responseParams = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String Error 
        {
            get
            {
                if (this._error == null)
                {
                    this._error = String.Empty;
                }

                return this._error;
            }
            set 
            {
                this._error = value;
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
