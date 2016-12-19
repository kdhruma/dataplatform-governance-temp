using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDM.Core.Exceptions
{
    /// <summary>
    /// Exception for duplicate objects found in a collection or array
    /// </summary>
    public class DuplicateObjectException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DuplicateObjectException class.
        /// </summary>
        public DuplicateObjectException()
        {

        }

        /// <summary>
        /// Initializes a new instance of the DuplicateObjectException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DuplicateObjectException(String message)
            : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance of the DuplicateObjectException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference</param>
        public DuplicateObjectException(String message, Exception innerException)
            : base(message, innerException)
        {

        }

        #endregion
    }
}
