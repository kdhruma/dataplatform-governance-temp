using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace RS.MDM.Validations
{
    /// <summary>
    /// Provides enumeration of different types of validation errors
    /// </summary>
    public enum ValidationErrorType
    {
        /// <summary>
        /// Indicates a critical error in the validation. This type of validation error may lead to fatal exceptions during runtime.
        /// </summary>
        Error,

        /// <summary>
        /// Indicated an information that is not critical
        /// </summary>
        Information,

        /// <summary>
        /// Indicates a warning that is not critical.
        /// </summary>
        Warning
    }

    /// <summary>
    /// Provides functionality to capture a validation error within RS.MDM Framework
    /// </summary>
    [Serializable]
    public sealed class ValidationError
    {
        #region Fields

        /// <summary>
        /// field for the message of validation error
        /// </summary>
        private string _message;

        /// <summary>
        /// field for the type of validation error
        /// </summary>
        private ValidationErrorType _validationErrorType = ValidationErrorType.Error;

        /// <summary>
        /// field for the name of the property that has caused validation error
        /// </summary>
        private string _propertyName;

        /// <summary>
        /// field for the object that failed to validate
        /// </summary>
        private RS.MDM.Object _source = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with message and source as input parameters
        /// </summary>
        /// <param name="message">Indicates the validation error message</param>
        /// <param name="source">Indicates the object that caused the validation error</param>
        public ValidationError(string message, RS.MDM.Object source)
            : this(message, ValidationErrorType.Error, null, source)
        {
        }

        /// <summary>
        /// Constructor with message, type of validation and source as input parameters
        /// </summary>
        /// <param name="message">Indicates the validation error message</param>
        /// <param name="validationErrorType">Indicates the type of validation error</param>
        /// <param name="source">Indicates the object that caused the validation error</param>
        public ValidationError(string message, ValidationErrorType validationErrorType, RS.MDM.Object source)
            : this(message, validationErrorType, null, source)
        {
        }

        /// <summary>
        /// Constructor with message, type of validation, propertyname and source as input parameters
        /// </summary>
        /// <param name="message">Indicates the validation error message</param>
        /// <param name="validationErrorType">Indicates the type of validation error</param>
        /// <param name="propertyName">Indicates the name of the property that caused the validation error</param>
        /// <param name="source">Indicates the object that caused the validation error</param>
        public ValidationError(string message, ValidationErrorType validationErrorType, string propertyName, RS.MDM.Object source)
        {
            this._message = message;
            this._validationErrorType = validationErrorType;
            this._propertyName = propertyName;
            this._source = source;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the consolidated message that represents the validation error
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} : {2}", new object[] { this._validationErrorType.ToString(), this._message });
        }

        #endregion

        #region  Properties

        /// <summary>
        /// Indicates the validation error message
        /// </summary>
        public string Message
        {
            get
            {
                return this._message;
            }
        }

        /// <summary>
        /// Indicates the type of validation error
        /// </summary>
        public ValidationErrorType ValidationErrorType
        {
            get
            {
                return this._validationErrorType;
            }
        }

        /// <summary>
        /// Indicates the name of the property that caused the validation error
        /// </summary>
        public string PropertyName
        {
            get
            {
                return this._propertyName;
            }
        }

        /// <summary>
        /// Indicates the object that caused the validation error
        /// </summary>
        public RS.MDM.Object Source
        {
            get
            {
                return this._source;
            }
        }


        /// <summary>
        /// Get a ListViewItem that represents the validation error
        /// </summary>
        /// <returns></returns>
        public System.Windows.Forms.ListViewItem ListViewItem
        {
            get
            {
                System.Windows.Forms.ListViewItem _listViewItem = new System.Windows.Forms.ListViewItem("", this._validationErrorType.ToString().ToLower());
                if (this._source != null)
                {
                    _listViewItem.SubItems.Add(this._source.GetType().FullName);
                }
                else
                {
                    _listViewItem.SubItems.Add("RS.MDM.Object");
                }
                _listViewItem.SubItems.Add(this._propertyName);
                _listViewItem.SubItems.Add(this._message);
                _listViewItem.Tag = this._source;
                return _listViewItem;
            }
        }

        #endregion

    }

}
