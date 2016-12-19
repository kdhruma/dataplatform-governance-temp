using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    /// <summary>
    /// Specifies the exception details occurs in the WCF service methods of MDM
    /// </summary>
    /// <remarks>Since this is not a MDM object it doesn't inherit from MDMObject</remarks>
    [DataContract]
    public class MDMExceptionDetails
    {
        #region Fields

        /// <summary>
        /// Field denoting message code of the exception
        /// </summary>
        private String _messageCode = String.Empty;

        /// <summary>
        /// Field denoting the Message of the Exception
        /// </summary>
        private String _message = String.Empty;

        /// <summary>
        /// Field denoting the Value of the immediate frames of the call stack
        /// </summary>
        private String _stackTrace = String.Empty;

        /// <summary>
        /// Field denoting the name of the source
        /// </summary>
        private String _source = String.Empty;

        /// <summary>
        /// Field denoting the method name
        /// </summary>
        private String _targetSite = String.Empty;

        /// <summary>
        /// Collection of custom message arguments to be used while creating message from MessageCode
        /// </summary>
        private Object[] _messageArguments = null;

        #endregion

        #region Constructors
        /// <summary>
        /// Parameterless constructors.
        /// </summary>
        public MDMExceptionDetails()
        {
        }

        /// <summary>
        /// Constructor with Message, StackTrace, Source and TargetSite as input parameters 
        /// </summary>
        /// <param name="messageCode">Indicates message code of the error</param>
        /// <param name="message">Indicates the message that describes the Error</param>
        /// <param name="stackTrace">Indicates the immediate frames of the call stack</param>
        /// <param name="source">Indicates the name of the application or the object that causes the error</param>
        /// <param name="targetSite">Indicates the method name that throws the current exception</param>
        public MDMExceptionDetails(String messageCode, String message, String stackTrace, String source, String targetSite)
        {
            _messageCode = messageCode;
            _message = message;
            _stackTrace = stackTrace;
            _source = source;
            _targetSite = targetSite;
        }

        /// <summary>
        /// Initializes a new instance of the MDMOperationException class with the specified parameters.
        /// </summary>
        /// <param name="messageCode">Message code to be used for this exception</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="source">The application or the object that causes the error.</param>
        /// <param name="stackTrace">The immediate frames on the call stack.</param>
        /// <param name="targetSite">The method that throws the current exception.</param>
        /// <param name="messageArguments">Custom parameters to be passed to generate message from message code</param>
        public MDMExceptionDetails(String messageCode, String message, String source, String stackTrace, String targetSite, params object[] messageArguments)
        {
            this._messageCode = messageCode;
            this._message = message;
            this._source = source;
            this._stackTrace = stackTrace;
            this._targetSite = targetSite;
            this._messageArguments = messageArguments;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Property denoting message code of the exception
        /// </summary>
        [DataMember]
        public String MessageCode
        {
            get
            {
                return _messageCode;
            }
            set
            {
                _messageCode = value;
            }
        }

        /// <summary>
        /// Property denoting the message that describes the error
        /// </summary>
        [DataMember]
        public String Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        /// <summary>
        /// Property denoting the immediate frames of the call stack
        /// </summary>
        [DataMember]
        public String StackTrace
        {
            get
            {
                return _stackTrace;
            }
            set
            {
                _stackTrace = value;
            }
        }

        /// <summary>
        ///  Property denoting the source name that cause the error
        /// </summary>
        [DataMember]
        public String Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }

        /// <summary>
        ///  Property denoting the method name that throws the exception
        /// </summary>
        [DataMember]
        public String TargetSite
        {
            get
            {

                return _targetSite;
            }
            set
            {
                _targetSite = value;
            }
        }

        /// <summary>
        /// Collection of custom message arguments to be used while creating message from MessageCode
        /// </summary>
        [DataMember]
        public Object[] MessageArguments
        {
            get { return _messageArguments; }
            set { _messageArguments = value; }
        }
        #endregion

        #region Methods

        #endregion
    }
}
