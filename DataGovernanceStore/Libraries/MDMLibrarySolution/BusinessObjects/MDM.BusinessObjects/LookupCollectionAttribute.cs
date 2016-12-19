using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MDM.BusinessObjects
{
    using MDM.Core;
    using MDM.Interfaces;

    internal class LookupCollectionAttribute : ILookupCollectionAttribute
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
        public LookupCollectionAttribute(IAttribute attribute)
        {
            if (attribute != null)
            {
                this._attribute = (Attribute)attribute;
            }
            else
            {
                throw new ArgumentNullException("LookupAttribute Initialization failed as Attribute is Null");
            }
        }

        #endregion

        #region GetValues Methods

        /// <summary>
        /// Retrieves current value of an attribute 
        /// </summary>
        /// <returns>Value</returns>
        public IValueCollection GetCurrentValueInstances()
        {
            IValueCollection iValues = null;

            if (this._attribute != null)
            {
                iValues = this._attribute.GetCurrentValues();
            }

            return iValues;
        }

        /// <summary>
        /// Retrieves inherited value of an attribute 
        /// </summary>
        /// <returns>Value</returns>
        public IValueCollection GetInheritedValueInstances()
        {
            IValueCollection iValues = null;

            if (this._attribute != null)
            {
                iValues = this._attribute.GetInheritedValues();
            }
            return iValues;
        }

        /// <summary>
        /// Retrieves current value of an attribute 
        /// </summary>
        /// <returns>Value</returns>
        public IValueCollection GetCurrentValueInstancesInvariant()
        {
            IValueCollection iValues = null;

            if (this._attribute != null)
            {
                iValues = this._attribute.GetCurrentValuesInvariant();
            }

            return iValues;
        }

        /// <summary>
        /// Retrieves inherited value of an attribute 
        /// </summary>
        /// <returns>Value</returns>
        public IValueCollection GetInheritedValueInstancesInvariant()
        {
            IValueCollection iValues = null;

            if (this._attribute != null)
            {
                iValues = this._attribute.GetInheritedValuesInvariant();
            }
            return iValues;
        }

        /// <summary>
        /// Gets the current lookup row as a dictionary based on the specified value reference id
        /// </summary>
        /// <param name="valueRefId">Specifies the value reference id of the lookup row</param>
        /// <returns>A dictionary representing the lookup row</returns>
        public Dictionary<String, String> GetCurrentLookupRowAsDictionary(Int32 valueRefId)
        {
            ValueCollection values = (ValueCollection)GetCurrentValueInstances();
            return GetExtendedValues(values, valueRefId);
        }

        /// <summary>
        /// Gets the inherited lookup row as a dictionary based on the specified value reference id
        /// </summary>
        /// <param name="valueRefId">Specifies the value reference id of the lookup row</param>
        /// <returns>A dictionary representing the lookup row</returns>
        public Dictionary<String, String> GetInheritedLookupRowAsDictionary(Int32 valueRefId)
        {
            ValueCollection values = (ValueCollection)GetInheritedValueInstances();
            return GetExtendedValues(values, valueRefId);
        }

        /// <summary>
        /// Gets the current lookup cell values across all rows based on the column name
        /// </summary>
        /// <param name="columnName">Specifies the lookup column name</param>
        /// <returns>A collection of lookup cell values</returns>
        public Collection<String> GetCurrentLookupCellValueCollection(String columnName)
        {
            ValueCollection values = (ValueCollection)GetCurrentValueInstances();
            return GetLookupCellValueCollection(values, columnName);
        }

        /// <summary>
        /// Gets the inherited lookup cell values across all rows based on the column name
        /// </summary>
        /// <param name="columnName">Specifies the lookup column name</param>
        /// <returns>A collection of lookup cell values</returns>
        public Collection<String> GetInheritedLookupCellValueCollection(String columnName)
        {
            ValueCollection values = (ValueCollection)GetInheritedValueInstances();
            return GetLookupCellValueCollection(values, columnName);
        }

        /// <summary>
        /// Gets the current lookup cell value based on the specified value reference id and column name
        /// </summary>
        /// <param name="valueRefId">Specifies the value reference id of the lookup row</param>
        /// <param name="columnName">Specifies the lookup column name</param>
        /// <returns>The lookup cell value</returns>
        public String GetCurrentLookupCellValue(Int32 valueRefId, String columnName)
        {
            Dictionary<String, String> extendedValues = GetCurrentLookupRowAsDictionary(valueRefId);
            return GetLookupCellValue(extendedValues, columnName);
        }

        /// <summary>
        /// Gets the inherited lookup cell value based on the specified value reference id and column name
        /// </summary>
        /// <param name="valueRefId">Specifies the value reference id of the lookup row</param>
        /// <param name="columnName">Specifies the lookup column name</param>
        /// <returns>The lookup cell value</returns>
        public String GetInheritedLookupCellValue(Int32 valueRefId, String columnName)
        {
            Dictionary<String, String> extendedValues = GetInheritedLookupRowAsDictionary(valueRefId);
            return GetLookupCellValue(extendedValues, columnName);
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
        public void AppendValuesInvariant(Object value)
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

        private Collection<String> GetLookupCellValueCollection(ValueCollection valueCollection, String columnName)
        {
            Collection<String> columnValues = new Collection<String>();
            
            if (valueCollection != null)
            {
                String lookupCellValue = null;

                foreach (Value value in valueCollection)
                {
                    lookupCellValue = GetLookupCellValue(value.ExtendedValues, columnName);
                    if (!String.IsNullOrWhiteSpace(lookupCellValue))
                    {
                        columnValues.Add(lookupCellValue);
                    }
                }
            }

            return columnValues;
        }

        private Dictionary<String, String> GetExtendedValues(ValueCollection valueCollection, Int32 valueRefId)
        {
            if (valueCollection != null)
            {
                foreach (Value value in valueCollection)
                {
                    if (value.ValueRefId == valueRefId)
                    {
                        return value.ExtendedValues;
                    }
                }
            }
            return null;
        }

        private String GetLookupCellValue(Dictionary<String, String> extendedValues, String columnName)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                throw new ArgumentNullException("columnName", "columnName cannot be empty or null");
            }

            if (extendedValues != null)
            {
                String extendedValue = String.Empty;
                if (extendedValues.TryGetValue(columnName, out extendedValue))
                {
                    return extendedValue;
                }
            }
            return String.Empty;
        }

        #endregion
    }
}
