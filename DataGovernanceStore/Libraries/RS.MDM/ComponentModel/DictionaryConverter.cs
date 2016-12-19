using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace RS.MDM.ComponentModel
{
    /// <summary>
    ///  Provides a set of enumerated values for a given object that implements IDictionaryProperties
    /// </summary>
    public class DictionaryConvertor : StringConverter
    {
        #region Override Methods

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter
        ///   is designed for when provided with a format context.
        /// </summary>
        /// <param name="context">
        ///    An System.ComponentModel.ITypeDescriptorContext that provides a format context
        ///     that can be used to extract additional information about the environment
        ///     from which this converter is invoked. This parameter or properties of this
        ///     parameter can be null.
        /// </param>
        /// <returns>
        ///     A System.ComponentModel.TypeConverter.StandardValuesCollection that holds
        ///     a standard set of valid values, or null if the data type does not support
        ///     a standard set of values.
        /// </returns>
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            IDictionaryProperties _selectedObject = context.Instance as IDictionaryProperties;
            string _propName = context.PropertyDescriptor.Name;
            Dictionary<int, string> _dictionary = _selectedObject.GetDictionary(_propName);
            return new StandardValuesCollection(_dictionary.Values);
        }

        /// <summary>
        ///     Returns whether the collection of standard values returned from System.ComponentModel.TypeConverter.GetStandardValues()
        ///     is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">
        /// An System.ComponentModel.ITypeDescriptorContext that provides a format context.        
        /// </param>
        /// <returns>
        ///     true if the System.ComponentModel.TypeConverter.StandardValuesCollection
        ///     returned from System.ComponentModel.TypeConverter.GetStandardValues() is
        ///     an exhaustive list of possible values; false if other values are possible.
        /// </returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        ///     Returns whether this object supports a standard set of values that can be
        ///     picked from a list.
        /// </summary>
        /// <param name="context">
        ///     An System.ComponentModel.ITypeDescriptorContext that provides a format context.
        /// </param>
        /// <returns>
        ///     true if System.ComponentModel.TypeConverter.GetStandardValues() should be
        ///     called to find a common set of values the object supports; otherwise, false.
        /// </returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        #endregion
    }
}
