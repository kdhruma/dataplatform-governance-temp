using System;
using System.Runtime.Serialization;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represent class to represent application message specifying the message code, type, message, etc.
    /// </summary>
    [DataContract]
    [KnownType(typeof(MDMObject))]  
    public class ApplicationMessage : MDMObject, IApplicationMessage 
    {
        #region Fields
        
        /// <summary>
        /// Field for code
        /// </summary>
        private String _code = String.Empty;

        /// <summary>
        /// Field for type
        /// </summary>
        private String _type = String.Empty;

        /// <summary>
        /// Field for type
        /// </summary>
        private String _message = String.Empty;

        #endregion

        #region Properties
        /// <summary>
        /// Property denoting code of the message
        /// </summary>
        [DataMember]
        public String Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
            }
        }

        /// <summary>
        /// Property denoting type of the message
        /// </summary>
        [DataMember]
        public String Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        /// <summary>
        /// Property denoting type of the message
        /// </summary>
        [DataMember]
        public String Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
        }
        #endregion

    }
}
