using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    /// <summary>
    /// Class provides methods to manipulate Collection Attribute Value.
    /// </summary>
    internal class CollectionAttribute : ICollectionAttribute
    {
        #region Fields

        /// <summary>
        /// Field denoting the values.
        /// </summary>
        private Attribute _attribute = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize CollectionAttribute object from attribute
        /// </summary>
        /// <param name="attribute">Indicates the CollectionAttribute</param>
        public CollectionAttribute(Attribute attribute)
        {
            if (attribute != null)
            {
                this._attribute = attribute;
            }
            else
            {
                throw new ArgumentNullException("Initialization failed CollectionAttribute as Attribute is Null");
            }
        }

        #endregion

        #region Public Methods

        #region Attribute Value Get

        /// <summary>
        /// Retrieves current values of an attribute under current entity
        /// </summary>
        /// <returns>Value</returns>
        public IValueCollection GetCurrentValueInstances()
        {
            IValueCollection iValueCollection = null;

            if (this._attribute != null)
            {
                iValueCollection = this._attribute.GetCurrentValues();
            }

            return iValueCollection;
        }

        /// <summary>
        /// Retrieves current values of an attribute under current entity
        /// </summary>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Value</returns>
        public IValueCollection GetCurrentValueInstances(LocaleEnum locale)
        {
            IValueCollection iValueCollection = null;

            if (this._attribute != null)
            {
                iValueCollection = this._attribute.GetCurrentValues(locale);
            }

            return iValueCollection;
        }

        /// <summary>
        /// Retrieves current values of an attribute under current entity
        /// </summary>
        /// <returns>Value</returns>
        public IValueCollection GetCurrentValueInstancesInvariant()
        {
            IValueCollection iValueCollection = null;

            if (this._attribute != null)
            {
                iValueCollection = this._attribute.GetCurrentValuesInvariant();
            }
            return iValueCollection;
        }

        /// <summary>
        /// Retrieves inherited values of an attribute under current entity
        /// </summary>
        /// <returns>Value</returns>
        public IValueCollection GetInheritedValueInstances()
        {
            IValueCollection iValueCollection = null;

            if (this._attribute != null)
            {
                iValueCollection = this._attribute.GetInheritedValues();
            }
            return iValueCollection;
        }

        /// <summary>
        /// Retrieves inherited values  of an attribute under current entity
        /// </summary>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Value</returns>
        public IValueCollection GetInheritedValueInstances(LocaleEnum locale)
        {
            IValueCollection iValueCollection = null;

            if (this._attribute != null)
            {
                iValueCollection = this._attribute.GetInheritedValues(locale);
            }
            return iValueCollection;
        }

        /// <summary>
        /// Retrieves inherited values of an attribute under current entity
        /// </summary>
        /// <returns>Value</returns>
        public IValueCollection GetInheritedValueInstancesInvariant()
        {
            IValueCollection iValueCollection = null;

            if (this._attribute != null)
            {
                iValueCollection = this._attribute.GetInheritedValues();
            }
            return iValueCollection;
        }

        /// <summary>
        /// Get the current values for the attribute.
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <returns>Returns the Invariant value based on the requested data type if the data type is valid</returns>
        public Collection<T> GetCurrentValues<T>()
        {
            return this.GetAttributeCurrentValues<T>(this._attribute.Locale, false);
        }

        /// <summary>
        /// Get the current invariant values for the attribute.
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <returns>Returns the current invariant value based on the requested data type if the data type is valid</returns>
        public Collection<T> GetCurrentValuesInvariant<T>()
        {
            return this.GetAttributeCurrentValues<T>(this._attribute.Locale, true);
        }

        /// <summary>
        /// Get the current values for the attribute.
        /// </summary>
        /// <typeparam name="T">Indicates the date type in which format want the result</typeparam>
        /// <param name="locale">Specifies Locale in which value should be formatted</param>
        /// <returns>Returns the Invariant value based on the requested data type if the data type is valid</returns>
        public Collection<T> GetCurrentValues<T>(LocaleEnum locale)
        {
            return this.GetAttributeCurrentValues<T>(locale, false);
        }

        #endregion

        #region Set/Append Attribute Overridden Values

        /// <summary>
        /// Overrides already existing value with new values.
        /// </summary>
        /// <param name="values">Specifies New value to set in overridden attribute collection</param>
        public void SetValues(IValueCollection values)
        {
            this._attribute.SetValue(values);
        }

        /// <summary>
        /// Overrides already existing value with new values.
        /// </summary>
        /// <param name="valueCollection">Specifies New value to set in overridden attribute collection</param>
        /// <param name="formatLocale">Specifies Locale in which values will be set</param>
        public void SetValues(IValueCollection valueCollection, LocaleEnum formatLocale)
        {
            this._attribute.SetValue(valueCollection, formatLocale);
        }

        /// <summary>
        /// Overrides already existing value with new values.
        /// </summary>
        /// <param name="values">Specifies New value to set in overridden attribute collection</param>
        public void SetValuesInvariant(IValueCollection values)
        {
            this._attribute.SetValueInvariant(values);
        }

        /// <summary>
        /// Overrides already existing value with new values.
        /// </summary>
        /// <param name="values">Specifies New value to add in overridden attribute collection</param>
        public void SetValues(Object[] values)
        {
            ValueCollection valuesObj = GetValueCollection(values);

            if (valuesObj.Count() > 0)
            {
                this._attribute.SetValue(valuesObj);
            }
        }

        /// <summary>
        /// Overrides already existing value with new values.
        /// </summary>
        /// <param name="values">Specifies New value to add in overridden attribute collection</param>
        /// <param name="locale">Specifies Locale in which values will be set</param>
        public void SetValues(Object[] values, LocaleEnum locale)
        {
            ValueCollection valuesObj = GetValueCollection(values);

            if (valuesObj.Count() > 0)
            {
                this._attribute.SetValue(valuesObj, locale);
            }
        }

        /// <summary>
        /// Overrides already existing value with new values.
        /// </summary>
        /// <param name="values">Specifies New value to add in overridden attribute collection</param>
        public void SetValuesInvariant(Object[] values)
        {
            ValueCollection valuesObj = GetValueCollection(values);

            if (valuesObj.Count() > 0)
            {
                this._attribute.SetValueInvariant(valuesObj);
            }
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// </summary>
        /// <param name="valueCollection">Specifies New value to add in overridden attribute collection</param>
        /// <exception cref="ArgumentNullException">ValueCollection cannot be null</exception>
        public void AppendValues(IValueCollection valueCollection)
        {
            this._attribute.AppendValue(valueCollection);
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// </summary>
        /// <param name="valueCollection">Specifies New value to add in overridden attribute collection</param>
        /// <param name="formatLocale">Specifies Locale in which values will be set</param>
        public void AppendValues(IValueCollection valueCollection, LocaleEnum formatLocale)
        {
            this._attribute.AppendValue(valueCollection, formatLocale);
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// </summary>
        /// <param name="valueCollection">Specifies New value to add in overridden attribute collection</param>
        public void AppendValuesInvariant(IValueCollection valueCollection)
        {
            this._attribute.AppendValueInvariant(valueCollection);
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// </summary>
        /// <param name="values">Specifies New value to add in overridden attribute collection</param>
        public void AppendValuesInvariant(Object[] values)
        {
            ValueCollection valuesObj = GetValueCollection(values);

            if (valuesObj.Count() > 0)
            {
                this._attribute.AppendValueInvariant(valuesObj);
            }
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// </summary>
        /// <param name="values">Specifies New value to add in overridden attribute collection</param>
        public void AppendValues(Object[] values)
        {
            ValueCollection valuesObj = GetValueCollection(values);

            if (valuesObj.Count() > 0)
            {
                this._attribute.AppendValue(valuesObj);
            }
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// </summary>
        /// <param name="values">Specifies New value to add in overridden attribute collection</param>
        /// <param name="locale">Specifies Locale in which values will be set</param>
        public void AppendValues(Object[] values, LocaleEnum locale)
        {
            ValueCollection valuesObj = GetValueCollection(values);

            if (valuesObj.Count() > 0)
            {
                this._attribute.AppendValue(valuesObj, locale);
            }
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// </summary>
        /// <param name="value">Specifies New value to add in overridden attribute collection</param>
        public void AppendValueInvariant(Object value)
        {
            if (value != null)
            {
                Value valueObj = new Value(value);
                valueObj.Action = ObjectAction.Update;
                this._attribute.AppendValueInvariant(valueObj);
            }
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// </summary>
        /// <param name="value">Specifies New value to append in overridden attribute collection</param>
        public void AppendValue(Object value)
        {
            if (value != null)
            {
                Value valueObj = new Value(value);
                valueObj.Action = ObjectAction.Update;
                this._attribute.AppendValue(valueObj);
            }
        }

        /// <summary>
        /// Appends new value in already existing values.
        /// </summary>
        /// <param name="value">Specifies New value to add in overridden attribute collection</param>
        /// <param name="locale">Specifies Locale in which values will be set</param>
        public void AppendValue(Object value, LocaleEnum locale)
        {
            if (value != null)
            {
                Value valueObj = new Value(value);
                valueObj.Action = ObjectAction.Update;
                this._attribute.AppendValue(valueObj, locale);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        private ValueCollection GetValueCollection(Object[] values)
        {
            ValueCollection valuesCollection = null;

            if (values != null)
            {
                valuesCollection = new ValueCollection();

                foreach (Object value in values)
                {
                    Value valueObj = new Value(value);
                    valueObj.Action = ObjectAction.Update;

                    valuesCollection.Add(valueObj);
                }
            }

            return valuesCollection;
        }

        private Collection<T> GetAttributeCurrentValues<T>(LocaleEnum locale, Boolean getInvariantValue)
        {
            Collection<T> result = new Collection<T>();

            if (this != null)
            {
                if (!this._attribute.IsCollection)
                {
                    throw new NotSupportedException(String.Format("{0} is not a collection attribute.Use GetCurrentValue<T>() method to get non collection attribute value", this._attribute.Name));
                }
                IValueCollection values = null;

                if (getInvariantValue)
                {
                    values = this._attribute.GetCurrentValuesInvariant();
                }
                else
                {
                    values = this._attribute.GetCurrentValues(locale);
                }

                foreach (IValue value in values)
                {
                    result.Add(this._attribute.GetAttributeValue<T>(value));
                }
            }
            return result;
        }

        #endregion
    }
}
