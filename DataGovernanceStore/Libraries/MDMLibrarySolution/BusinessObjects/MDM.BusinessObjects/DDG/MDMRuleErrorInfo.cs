using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects.DDG
{
    /// <summary>
    /// Error information of MDM rule execution
    /// </summary>
    [DataContract]
    public class MDMRuleErrorInfo
    {
        #region Fields

        private String _errorMsg;
        private String _errorCode;
        private Object[] _parameters;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MDMRuleErrorInfo()
        {
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="errorCode">Indicates error  code </param>
        /// <param name="errorMessage">Indicates error message</param>
        /// <param name="parameters">Indicates parameters</param>
        public MDMRuleErrorInfo(String errorCode, String errorMessage, Object[] parameters = null)
        {
            if (!String.IsNullOrWhiteSpace(errorMessage) && parameters != null && parameters.Length > 0)
            {
                errorMessage = String.Format(errorMessage, parameters);
            }
            this.ErrorCode = errorCode;
            this.ErrorMsg = errorMessage;
            this.Parameters = parameters;
        }

        #endregion
                
        #region Properties
        /// <summary>
        /// Property denoting last error message received while evaluating business rule
        /// </summary>
        [DataMember]
        public String ErrorMsg 
        {
            get
            {
                return this._errorMsg;
            }
            set
            {
                this._errorMsg = value;
            }
        }

        /// <summary>
        /// Property denoting last error code received while evaluating business rule
        /// </summary>
        [DataMember]
        public String ErrorCode 
        {
            get
            {
                return this._errorCode;
            }
            set
            {
                this._errorCode = value;
            }
        }

        /// <summary>
        /// Property denoting parameters for the BR error message
        /// </summary>
        [DataMember]
        public Object[] Parameters
        {
            get
            {
                return this._parameters;
            }
            set
            {
                this._parameters = value;
            }
        }
        #endregion
    }
}
