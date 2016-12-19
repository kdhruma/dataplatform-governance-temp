using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace RS.MDM
{
    /// <summary>
    /// Provides base class to capture audit details and identity of an object
    /// </summary>
    [DataContract(Namespace = "http://Riversand.MDM.Services")]
    public class Audit
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        private DateTime _createDateTime;

        /// <summary>
        /// 
        /// </summary>
        private DateTime _modDateTime;

        /// <summary>
        /// 
        /// </summary>
        private string _createUser;

        /// <summary>
        /// 
        /// </summary>
        private string _modUser;

        /// <summary>
        /// 
        /// </summary>
        private string _createProgram;

        /// <summary>
        /// 
        /// </summary>
        private string _modProgram;

        #endregion

        #region Constructors

        /// <summary>
        /// Parameterless Constructor
        /// </summary>
        public Audit()
            : base()
        { 
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates the create timestamp of an object
        /// </summary>
        [DataMember]
        [Description("Indicates the create timestamp of an object")]
        [Category("Audit")]
        public DateTime CreateDateTime
        {
            get
            {
                return this._createDateTime;
            }
            set
            {
                this._createDateTime = value;
            }
        }

        /// <summary>
        /// Indicates the last modified timestamp of an object
        /// </summary>
        [DataMember]
        [Description("Indicates the last modified timestamp of an object")]
        [Category("Audit")]
        public DateTime ModDateTime
        {
            get
            {
                return this._modDateTime;
            }
            set
            {
                this._modDateTime = value;
            }
        }

        /// <summary>
        /// Indicates the user that has created an object
        /// </summary>
        [DataMember]
        [Description("Indicates the user that has created an object")]
        [Category("Audit")]
        public string CreateUser
        {
            get
            {
                return this._createUser; 
            }
            set
            {
                this._createUser = value;
            }
        }

        /// <summary>
        /// Indicates the user that has modified an object
        /// </summary>
        [DataMember]
        [Description("Indicates the user that has modified an object")]
        [Category("Audit")]
        public string ModUser
        {
            get
            {
                return this._modUser;
            }
            set
            {
                this._modUser = value;
            }
        }

        /// <summary>
        /// Indicates the program that has created an object
        /// </summary>
        [DataMember]
        [Description("Indicates the program that has created an object")]
        [Category("Audit")]
        public string CreateProgram
        {
            get
            {
                return this._createProgram; 
            }
            set
            {
                this._createProgram = value ;
            }
        }

        /// <summary>
        /// Indicates the program that has modified an object
        /// </summary>
        [DataMember]
        [Description("Indicates the program that has modified an object")]
        [Category("Audit")]
        public string ModProgram
        {
            get
            {
                return this._modProgram;
            }
            set
            {
                this._modProgram = value;
            }
        }

        #endregion

    }
}
