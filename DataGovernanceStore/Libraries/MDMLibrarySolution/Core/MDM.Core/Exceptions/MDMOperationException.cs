using MDM.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Core.Exceptions
{
    /// <summary>
    /// Exception consisting the MDM operation exception details
    /// </summary>
    /// <remarks>The fault exception details of MDM operations cannot be sent to the UIP. In order to access these details, 
    /// the details have been added to MDMOperationException inheriting from Exception class and passed to UIP.
    /// </remarks>
    public class MDMOperationException : Exception
    {
        #region Fields

        /// <summary>
        /// Represents message code that describes the current exception.
        /// </summary>
        private String _messageCode = String.Empty;

        /// <summary>
        /// Represents message that describes the current exception.
        /// </summary>
        private String _message = String.Empty;

        /// <summary>
        /// Represents name of the application or the object that causes the error.
        /// </summary>
        private String _source = String.Empty;

        /// <summary>
        /// Represents the immediate frames on the call stack.
        /// </summary>
        private String _stackTrace = String.Empty;

        /// <summary>
        /// Represents method that throws the current exception.
        /// </summary>
        private String _targetSite = String.Empty;

        /// <summary>
        /// Represents the System.Exception instance that caused the current exception.
        /// </summary>
        private Exception _innerException = new Exception();

        /// <summary>
        /// Collection of custom message arguments to be used while creating message from MessageCode
        /// </summary>
        private Object[] _messageArguments = null;

        /// <summary>
        ///  Indicates Reason type
        /// </summary>
        private ReasonType _reasonType = ReasonType.Unknown;

        /// <summary>
        /// Indicates unique identifier for rule map context  
        /// </summary>
        private Int32 _ruleMapContextId = -1;

        /// <summary>
        /// Indicates unique identifier for rule
        /// </summary>
        private Int32 _ruleId = -1;

        /// <summary>
        /// 
        /// </summary>
        private ExecutionContext _executionContext = null;

        #endregion

        #region Properties

        /// <summary>
        /// Represents message code that describes the current exception.
        /// </summary>
        public String MessageCode
        {
            get
            {
                return _messageCode;
            }
        }

        /// <summary>
        /// Represents message that describes the current exception.
        /// </summary>
        public override String Message
        {
            get
            {
                return _message;
            }
        }

        /// <summary>
        /// Represents name of the application or the object that causes the error.
        /// </summary>
        public override String Source
        {
            get
            {
                return _source;
            }
        }

        /// <summary>
        /// Represents the immediate frames on the call stack.
        /// </summary>
        public override String StackTrace
        {
            get
            {
                return _stackTrace;
            }
        }

        /// <summary>
        /// Represents method that throws the current exception.
        /// </summary>
        public new string TargetSite
        {
            get 
            { 
                return _source; 
            }
        }

        /// <summary>
        /// Represents the System.Exception instance that caused the current exception.
        /// </summary>
        public new Exception InnerException
        {
            get
            {
                return _innerException;
            }
        }

        /// <summary>
        /// Collection of custom message arguments to be used while creating message from MessageCode
        /// </summary>
        public Object[] MessageArguments
        {
            get { return _messageArguments; }
            set { _messageArguments = value; }
        }

        /// <summary>
        /// Represents the System.Exception instance that caused the current exception.
        /// </summary>
        public ExecutionContext ExecutionContext
        {
            get
            {
                return _executionContext;
            }
            set
            {
                _executionContext = value;
            }
        }

        /// <summary>
        /// Indicates unique identifier for rule map context
        /// </summary>
        public Int32 RuleMapContextId
        {
            get
            {
                return this._ruleMapContextId;
            }
            set
            {
                this._ruleMapContextId = value;
            }
        }

        /// <summary>
        /// Indicates unique identifier for rule
        /// </summary>
        public Int32 RuleId
        {
            get
            {
                return this._ruleId;
            }
            set
            {
                this._ruleId = value;
            }
        }

        /// <summary>
        ///  Indicates reason type
        /// </summary>
        public ReasonType ReasonType
        {
            get
            {
                return this._reasonType;
            }
            set
            {
                this._reasonType = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MDMOperationException class.
        /// </summary>
        public MDMOperationException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the MDMOperationException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MDMOperationException(String message)
        {
            this._message = message;
        }

        /// <summary>
        /// Initializes a new instance of the MDMOperationException class with the specified parameters.
        /// </summary>
        /// <param name="messageCode">Message code to be used for this exception</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="source">The application or the object that causes the error.</param>
        /// <param name="stackTrace">The immediate frames on the call stack</param>
        /// <param name="targetSite">The method that throws the current exception.</param>
        public MDMOperationException(String messageCode, String message, String source, String stackTrace, String targetSite)
        {
            this._messageCode = messageCode;
            this._message = message;
            this._source = source;
            this._stackTrace = stackTrace;
            this._targetSite = targetSite;
        }

        /// <summary>
        /// Initializes a new instance of the MDMOperationException class with the specified parameters.
        /// </summary>
        /// <param name="messageCode">Message code to be used for this exception</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="source">The application or the object that causes the error.</param>
        /// <param name="stackTrace">The immediate frames on the call stack</param>
        /// <param name="targetSite">The method that throws the current exception.</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        public MDMOperationException(String messageCode, String message, String source, String stackTrace, String targetSite, ReasonType reasonType, Int32 ruleMapContextId)
            : this(messageCode, message, source, stackTrace, targetSite)
        {
            this._reasonType = reasonType;
            this._ruleMapContextId = ruleMapContextId;
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
        public MDMOperationException(String messageCode, String message, String source, String stackTrace, String targetSite, params object[] messageArguments)
        {
            this._messageCode = messageCode;
            this._message = message;
            this._source = source;
            this._stackTrace = stackTrace;
            this._targetSite = targetSite;
            this._messageArguments = messageArguments;
        }

        /// <summary>
        /// Initializes a new instance of the MDMOperationException class with the specified parameters.
        /// </summary>
        /// <param name="messageCode">Message code to be used for this exception</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="source">The application or the object that causes the error.</param>
        /// <param name="stackTrace">The immediate frames on the call stack.</param>
        /// <param name="targetSite">The method that throws the current exception.</param>
        /// <param name="reasonType">Indicates type of the reason.</param>
        /// <param name="ruleMapContextId">Indicates rule map context identifier.</param>
        /// <param name="ruleId">Indicates rule identifier.</param>
        /// <param name="messageArguments">Custom parameters to be passed to generate message from message code</param>
        public MDMOperationException(String messageCode, String message, String source, String stackTrace, String targetSite, ReasonType reasonType, Int32 ruleMapContextId,Int32 ruleId, params object[] messageArguments)
            : this(messageCode, message, source, stackTrace, targetSite, messageArguments)
        {
            _reasonType = reasonType;
            _ruleMapContextId = ruleMapContextId;
            _ruleId = ruleId;
        }

        /// <summary>
        /// Initializes a new instance of the MDMOperationException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference</param>
        public MDMOperationException(String message, Exception innerException)
        {
            this._message = message;
            this._innerException = innerException;
        }

        #endregion

        #region Methods

        #endregion
    }
}