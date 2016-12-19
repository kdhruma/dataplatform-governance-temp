using System;
using System.Collections.Generic;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Represents an internal class that gives utility methods for lookup attribute.
    /// </summary>
    internal class LookupAttribute : ILookupAttribute
    {
        #region Fields

        /// <summary>
        /// Field denoting the lookup attribute.
        /// </summary>
        private Attribute _attribute = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize LookupAttribute object from attribute
        /// </summary>
        /// <param name="attribute">Indicates the LookupAttribute</param>
        public LookupAttribute(IAttribute attribute)
        {
            if (attribute != null)
            {
                this._attribute = (Attribute)attribute;
            }
            else
            {
                throw new ArgumentNullException("Initialization failed LookupAttribute as Attribute is Null");
            }
        }

        #endregion

        #region Value Get

        /// <summary>
        /// Indicates a method for getting current value of the the lookup attribute
        /// </summary>
        /// <returns>Returns a current value of the the lookup attribute</returns>
        public IValue GetCurrentValueInstance()
        {
            IValue iValue = null;

            if (this._attribute != null)
            {
                iValue = this._attribute.GetCurrentValueInstance();
            }

            return iValue;
        }

        /// <summary>
        /// Indicates a method for getting inherited value of the the lookup attribute
        /// </summary>
        /// <returns>Returns a inherited value of the the lookup attribute</returns>
        public IValue GetInheritedValueInstance()
        {
            IValue iValue = null;

            if (this._attribute != null)
            {
                iValue = this._attribute.GetInheritedValueInstance();
            }
            return iValue;
        }

        /// <summary>
        /// Indicates a method for getting current locale invariant value of the the lookup attribute
        /// </summary>
        /// <returns>Returns a current locale invariant value of the the lookup attribute</returns>
        public IValue GetCurrentValueInstanceInvariant()
        {
            IValue iValue = null;

            if (this._attribute != null)
            {
                iValue = this._attribute.GetCurrentValueInstanceInvariant();
            }

            return iValue;
        }

        /// <summary>
        /// Indicates a method for getting inherited locale invariant value of the the lookup attribute
        /// </summary>
        /// <returns>Returns a inherited locale invariant value of the the lookup attribute</returns>
        public IValue GetInheritedValueInstanceInvariant()
        {
            IValue iValue = null;

            if (this._attribute != null)
            {
                iValue = this._attribute.GetInheritedValueInstanceInvariant();
            }
            return iValue;
        }

        /// <summary>
        /// Indicates a method for getting current display value of the the lookup attribute
        /// </summary>
        /// <returns>Returns a string representation of current display value of the the lookup attribute</returns>
        public String GetCurrentDisplayValue()
        {
            IValue iValue = GetCurrentValueInstance();

            if (iValue != null)
            {
                return iValue.GetDisplayValue();
            }

            return null;
        }

        /// <summary>
        /// Indicates a method for getting current export value
        /// </summary>
        /// <returns>Returns a string representation of current export value</returns>
        public String GetCurrentExportValue()
        {
            IValue iValue = GetCurrentValueInstance();

            if (iValue != null)
            {
                return iValue.GetExportValue();
            }

            return null;
        }

        /// <summary>
        /// Indicates a method for getting inherited display value
        /// </summary>
        /// <returns>Returns a string representation of inherited display value</returns>
        public String GetInheritedDisplayValue()
        {
            IValue iValue = GetInheritedValueInstance();

            if (iValue != null)
            {
                return iValue.GetDisplayValue();
            }

            return null;
        }

        /// <summary>
        /// Indicates a method for getting inherited export value
        /// </summary>
        /// <returns>Returns a string representation of inherited export value</returns>
        public String GetInheritedExportValue()
        {
            IValue iValue = GetInheritedValueInstance();

            if (iValue != null)
            {
                return iValue.GetExportValue();
            }

            return null;
        }

        /// <summary>
        /// Indicates a method for getting a dictionary of the current lookup rows
        /// </summary>
        /// <returns>Returns a dictionary of the current lookup rows</returns>
        public Dictionary<String, String> GetCurrentLookupRowAsDictionary()
        {
            Value value = (Value)GetCurrentValueInstance();

            if (value != null && value.ExtendedValues != null)
            {
                return value.ExtendedValues;
            }

            return null;
        }

        /// <summary>
        /// Indicates a method for getting a dictionary of the inherited lookup rows
        /// </summary>
        /// <returns>Returns a dictionary of the inherited lookup rows</returns>
        public Dictionary<String, String> GetInheritedLookupRowAsDictionary()
        {
            Value value = (Value)GetInheritedValueInstance();

            if (value != null && value.ExtendedValues != null)
            {
                return value.ExtendedValues;
            }

            return null;
        }

        /// <summary>
        /// Indicates a method for getting a lookup cell's current value by name
        /// </summary>
        /// <param name="columnName">Represents a name of the lookup cell</param>
        /// <returns>Returns a lookup cell's current value</returns>
        public String GetCurrentLookupCellValue(String columnName)
        {
            IValue iValue = GetCurrentValueInstance();
            return GetExtendedValue((Value)iValue, columnName);
        }

        /// <summary>
        /// Indicates a method for getting a lookup cell's inherited value by name
        /// </summary>
        /// <param name="columnName">Represents a name of the lookup cell</param>
        /// <returns>Returns a lookup cell's inherited value</returns>
        public String GetInheritedLookupCellValue(String columnName)
        {
            IValue iValue = GetInheritedValueInstance();
            return GetExtendedValue((Value)iValue, columnName);
        }

        #endregion

        #region Value Set

        /// <summary>
        /// Indicates a method for overriding a lookup attribute's value using a lookup attribute's locale
        /// </summary>
        /// <param name="value">Specifies New value to set in overridden attribute</param>
        public void SetValue(IValue value)
        {
            this._attribute.SetValue(value);
        }

        /// <summary>
        /// Indicates a method for overriding a lookup attribute's value using a provided locale
        /// </summary>
        /// <param name="value">Specifies New value to set in overridden attribute</param>
        /// <param name="formatLocale">Specifies Locale in which value will be set</param>
        public void SetValue(IValue value, LocaleEnum formatLocale)
        {
            this._attribute.SetValue(value, formatLocale);
        }

        /// <summary>
        /// Indicates a method for overriding a lookup attribute's value in a locale invariant way
        /// </summary>
        /// <param name="value">Specifies New value to set in overridden attribute collection</param>
        public void SetValueInvariant(IValue value)
        {
            this._attribute.SetValueInvariant(value);
        }

        /// <summary>
        /// Indicates a method for overriding a lookup attribute's value from a provided object
        /// </summary>
        /// <param name="value">Specifies New value to add in overridden attribute</param>
        public void SetValue(Object value)
        {
            this._attribute.SetValue(value);
        }

        /// <summary>
        /// Indicates a method for overriding a lookup attribute's display value
        /// </summary>
        /// <param name="displayValue">Specifies display value</param>
        public void SetDisplayValue(String displayValue)
        {
            if (this._attribute != null)
            {
                IValue iValue = this._attribute.GetCurrentValueInstance();
                if (iValue != null)
                {
                    iValue.SetDisplayValue(displayValue);
                }
            }
        }

        /// <summary>
        /// Indicates a method for overriding a lookup attribute's export value
        /// </summary>
        /// <param name="exportValue">Specifies export value</param>
        public void SetExportValue(String exportValue)
        {
            if (this._attribute != null)
            {
                IValue iValue = this._attribute.GetCurrentValueInstance();
                if (iValue != null)
                {
                    iValue.SetDisplayValue(exportValue);
                }
            }
        }

        #endregion

        #region Private Methods

        private String GetExtendedValue(Value value, String columnName)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName", "columnName cannot be empty or null");
            }

            if (value != null)
            {
                Dictionary<String, String> extendedValues = value.ExtendedValues;
                if (extendedValues != null)
                {
                    String extendedValue = String.Empty;
                    if (extendedValues.TryGetValue(columnName, out extendedValue))
                    {
                        return extendedValue;
                    }
                }
            }
            return String.Empty;
        }

        #endregion
    }
}
