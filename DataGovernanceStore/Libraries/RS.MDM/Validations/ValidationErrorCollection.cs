using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace RS.MDM.Validations
{
    /// <summary>
    /// Provides functionality to aggregate validation exceptions
    /// </summary>
    [Serializable]
    public sealed class ValidationErrorCollection : Collection<ValidationError>
    {
        #region Constructors

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public ValidationErrorCollection()
        {
        }

        /// <summary>
        /// Constructor with list of validation errors as input
        /// </summary>
        /// <param name="value">An array of validation errors</param>
        public ValidationErrorCollection(IEnumerable<ValidationError> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            this.AddRange(value);
        }

        /// <summary>
        /// Constructor with list of validation errors as input
        /// </summary>
        /// <param name="value">An array of validation errors</param>
        public ValidationErrorCollection(ValidationErrorCollection value)
        {
            this.AddRange(value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a validation error to the collection
        /// </summary>
        /// <param name="message">Indicates the validation error message</param>
        /// <param name="validationErrorType">Indicates the type of validation error</param>
        /// <param name="propertyName">Indicates the name of the property that caused the validation error</param>
        /// <param name="source">Indicates the object that caused the validation error</param>
        public void Add(string message, ValidationErrorType validationErrorType, string propertyName, RS.MDM.Object source)
        {
            ValidationError _validationError = new ValidationError(message, validationErrorType, propertyName, source);
            this.Add(_validationError);
        }

        /// <summary>
        /// Adds an array of validation errors to the collection
        /// </summary>
        /// <param name="value">Indicates an array of validation errors</param>
        public void AddRange(IEnumerable<ValidationError> value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            foreach (ValidationError error in value)
            {
                base.Add(error);
            }
        }

        /// <summary>
        /// Inserts a validation error at a given index location
        /// </summary>
        /// <param name="index">Indicates the index location where the item needs to be inserted</param>
        /// <param name="item">Indicates the item (validation error) that needs to be inserted into the collection</param>
        protected override void InsertItem(int index, ValidationError item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            base.InsertItem(index, item);
        }

        /// <summary>
        /// Sets an item at a known index location
        /// </summary>
        /// <param name="index">Indicates the index location where the item needs to be set</param>
        /// <param name="item">Indicates the item (validation error) that needs to be set</param>
        protected override void SetItem(int index, ValidationError item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            base.SetItem(index, item);
        }

        /// <summary>
        /// Return an array of validation errors
        /// </summary>
        /// <returns>An array of validation errors</returns>
        public ValidationError[] ToArray()
        {
            ValidationError[] _errorArray = new ValidationError[this.Count];
            base.CopyTo(_errorArray, 0);
            return _errorArray;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a boolean value that indicates if a validation error exists in the collection.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                if (this.Count > 0)
                {
                    foreach (ValidationError _error in this)
                    {
                        if (_error != null && _error.ValidationErrorType == ValidationErrorType.Error)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }


        /// <summary>
        /// Gets a boolean value that indicates if a validation warning exists in the collection.
        /// </summary>
        public bool HasWarnings
        {
            get
            {
                if (this.Count > 0)
                {
                    foreach (ValidationError _error in this)
                    {
                        if ((_error != null) && _error.ValidationErrorType == ValidationErrorType.Warning)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Gets an array of ListViewItems that indicates the validation errors
        /// </summary>
        public System.Windows.Forms.ListViewItem[] ListViewItems
        {
            get
            {
                List<System.Windows.Forms.ListViewItem> _listViewItems = new List<System.Windows.Forms.ListViewItem>();
                foreach (ValidationError _error in this)
                {
                    if (_error != null)
                    {
                        _listViewItems.Add(_error.ListViewItem);
                    }
                }
                return _listViewItems.ToArray();
            }
        }

        #endregion
    }




}
